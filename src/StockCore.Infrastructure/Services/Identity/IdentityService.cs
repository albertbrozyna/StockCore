using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockCore.Application.Common.Interfaces;
using StockCore.Domain.Entities;
using StockCore.Domain.Enums;
using StockCore.Domain.Exceptions;
using System.Linq;

namespace StockCore.Infrastructure.Services.Identity
{
    public class IdentityService(UserManager<User> userManager) : IIdentityService
    {
        public async Task<Result> AddRoleToUser(Guid userId, UserRole newRole,CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

            if(user == null)
            {
                throw new UserNotFoundException(userId); 
            }
            // Check if user already doesn't have this role
            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Contains(newRole.ToString()))
            {
                return Result.Failure($"User already have role: {newRole.ToString()}");
            }

            // Add role to user

            var result = await userManager.AddToRoleAsync(user, newRole.ToString());

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errorMessages);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteRoleFromUser(Guid userId, UserRole roleToDelete, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(user => user.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }
            // Check if user have this role
            var userRoles = await userManager.GetRolesAsync(user);

            if (!userRoles.Contains(roleToDelete.ToString()))
            {
                return Result.Failure($"User doesn't have role: {roleToDelete.ToString()}");
            }
           
            // Delete role from user
            var result = await userManager.RemoveFromRoleAsync(user,roleToDelete.ToString());

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result.Failure(errorMessages);
            }

            return Result.Success();
        }


        public async Task<User?> GetUserById(Guid userId)
        {
            return await userManager.FindByIdAsync(userId.ToString());
        }

        public Task<User?> GetUserById(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetUserRoles(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            return await userManager.GetRolesAsync(user);
        }

        public async Task<Result> UpdateUserProfilePhoto(Guid userId, string profilePhotoUrl)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Result.Failure("User not found");
            }

            user.UpdateProfilePhotoUrl(profilePhotoUrl);

            IdentityResult result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure("Failed to update user");
            }

            return Result.Success();
        }


    }
}
