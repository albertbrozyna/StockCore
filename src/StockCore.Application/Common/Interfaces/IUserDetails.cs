namespace StockCore.Application.Common.Interfaces
{
    public interface IUserDetails
    {
        Guid UserId { get; }
        Guid TenantId { get; }
    }
}
