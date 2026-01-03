using Microsoft.AspNetCore.Http;
using StockCore.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockCore.Api
{
    public class CurrentUserDetails : IUserDetails
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public const string TenantClaimType = "tenant_id"; 
        public CurrentUserDetails(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var sub = user?.FindFirstValue(ClaimTypes.NameIdentifier)
                          ?? user?.FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var userId))
                {
                    throw new UnauthorizedAccessException("User context is missing or invalid.");
                }

                return userId;
            }
            set => throw new NotSupportedException("UserId is read-only from token.");
        }

        public Guid TenantId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var tenantClaim = user?.FindFirstValue(TenantClaimType);

                if (string.IsNullOrEmpty(tenantClaim) || !Guid.TryParse(tenantClaim, out var tenantId))
                {
                    throw new UnauthorizedAccessException("Tenant context is missing. Access denied.");
                }

                return tenantId;
            }
            set => throw new NotSupportedException("TenantId is read-only from token.");
        }
    }
}