using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using StockCore.Application.Common.Interfaces;
using StockCore.Application.Common.Interfaces.Authentication;
using StockCore.Domain.Entities;
using StockCore.Domain.Exceptions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockCore.Infrastructure.Services
{
    public class TokenService(
        IConfiguration configuration,
        IAppDbContext appDbContext,
        IIdentityService identityService,
        UserManager<User> userManager) : ITokenService
    {
        public string GenerateAccessToken(User user, IEnumerable<string> roles)
        {
            var secretKey = configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("JWT Secret Key not found in configuration.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Build list of claims
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token Id
                new("tenant_id", user.TenantId.ToString()), // Tenant Id
                new("email_verified", user.EmailConfirmed.ToString().ToLower())
            };

            // Add user roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpiryMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };

            var handler = new JsonWebTokenHandler();
            return handler.CreateToken(tokenDescriptor);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == userId,cancellationToken: cancellationToken);

            if(user == null){
                throw new UserNotFoundException(userId);
            }

            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var tokenString = Convert.ToBase64String(randomNumber);

            var refreshToken = new RefreshToken(
                tokenString,
                DateTime.UtcNow.AddDays(7),
                userId,
                user.TenantId
            );

            appDbContext.RefreshTokens.Add(refreshToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return tokenString;
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshAsync(string token, CancellationToken cancellationToken)
        {
            var storedToken = await appDbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.IsExpired())
            {
                throw new SecurityTokenException("Invalid or expired refresh token.");
            }

            var user = await appDbContext.Users
                .FirstOrDefaultAsync(u => u.Id == storedToken.UserId, cancellationToken);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var userRoles = await identityService.GetUserRoles(user.Id);
            
            // Revoke old token
            storedToken.Revoke();

            var newAccessToken = GenerateAccessToken(user, userRoles);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id, cancellationToken);

            return (newAccessToken, newRefreshToken);
        }
    }
}