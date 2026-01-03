using Microsoft.AspNetCore.Identity;
using StockCore.Domain.Enums;
using StockCore.Domain.Interfaces;
namespace StockCore.Domain.Entities
{
    public class User : IdentityUser<Guid>, ITenantEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastLogin { get; private set; }
        public string? AvatarUrl { get; private set; }
        public ICollection<RefreshToken>RefreshTokens { get; private set; }
        public Guid TenantId { get; set; }
        public bool IsAcceptedByCompany { get; set; }

        public User() { }

        public User(string firstName, string lastName, string email, string phoneNumber,Guid tenantId)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            PhoneNumber = phoneNumber;
            IsDeleted = false;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            TenantId = tenantId;

        }
        public void BlockUser()
        {
            IsActive = false;
        }

        public void UnblockUser()
        {
            IsActive = true;
        }
        public void AcceptUserToCompany()
        {
           IsAcceptedByCompany = true;
        }
        public void DeleteUserFromCompany()
        {
            IsAcceptedByCompany = false;
        }

        public void DeleteUser()
        {
            IsDeleted = true;
        }

        public void SetLastLoginNow()
        {
            LastLogin = DateTime.Now;
        }

        public void UpdateProfilePhotoUrl(string profilePhotoUrl)
        {
            this.AvatarUrl = profilePhotoUrl;
        }

        public bool IsEmailVerified() => EmailConfirmed;

        public void VerifyEmail()
        {
            EmailConfirmed = true;
        }
    }
}
