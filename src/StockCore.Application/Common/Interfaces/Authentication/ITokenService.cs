using StockCore.Domain.Entities;

namespace StockCore.Application.Common.Interfaces.Authentication
{
    public interface ITokenService
    {
        public string GenerateAccessToken(User user, IEnumerable<string> roles);
        public Task<string> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken);
        Task<(string AccessToken, string RefreshToken)> RefreshAsync(string token, CancellationToken cancellationToken);

    }
}
