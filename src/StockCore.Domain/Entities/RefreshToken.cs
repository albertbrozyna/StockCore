using StockCore.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class RefreshToken : ITenantEntity
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; }

        public string Token { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public bool IsRevoked { get; private set; }
        public Guid TenantId { get; set; }

        private RefreshToken() { } 

        public RefreshToken(string token, DateTime expiresAt, Guid userId,Guid tenantId)
        {
            Id = Guid.NewGuid();
            Token = token;
            ExpiresAt = expiresAt;
            UserId = userId;
            IsRevoked = false;
            TenantId = tenantId;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
    }
}
