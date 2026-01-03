using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using StockCore.Domain.Entities;
using StockCore.Domain.Enums;

namespace StockCore.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> UpdateUserProfilePhoto(Guid userId, string profilePhotoUrl);

        Task<IList<string>> GetUserRoles(Guid userId);
        Task<Result> AddRoleToUser(Guid userId, UserRole newRole, CancellationToken cancellationToken);
        Task<Result> DeleteRoleFromUser(Guid userId, UserRole roleToDelete, CancellationToken cancellationToken);
        Task<User?> GetUserById(string userId);
    }
}
