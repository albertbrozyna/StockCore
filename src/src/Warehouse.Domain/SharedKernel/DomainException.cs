namespace Warehouse.Domain.SharedKernel;

/// <summary>Thrown when a domain invariant is violated.</summary>
public sealed class DomainException : Exception
{
    public DomainException(Error error)
        : base(error?.Message)
    {
        Error = error!;
    }

    public Error Error { get; }
}
