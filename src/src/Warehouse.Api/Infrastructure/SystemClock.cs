using Warehouse.Api.Application.Abstractions;

namespace Warehouse.Api.Infrastructure;

/// <summary>Production <see cref="IClock"/> backed by <see cref="TimeProvider.System"/>.</summary>
public sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => TimeProvider.System.GetUtcNow();

    public DateOnly Today(string timeZone)
    {
        var zone = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
        return DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(UtcNow, zone).DateTime);
    }
}
