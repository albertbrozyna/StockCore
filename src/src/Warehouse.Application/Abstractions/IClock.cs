namespace Warehouse.Application.Abstractions;

/// <summary>Source of time. The domain never reads the system clock directly.</summary>
public interface IClock
{
    DateTimeOffset UtcNow { get; }

    /// <summary>Current date in the given IANA time zone.</summary>
    DateOnly Today(string timeZone);
}
