using CSharpFunctionalExtensions;
using StockCore.Domain.Entities;

namespace StockCore.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> UpdateUserProfilePhoto(Guid userId, string profilePhotoUrl);

        Task<IList<string>> GetUserRoles(Guid userId);

        Task<User?> GetUserById(string userId);
    }
}
