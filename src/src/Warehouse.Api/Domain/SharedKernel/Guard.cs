using System.Numerics;
using System.Runtime.CompilerServices;
using Warehouse.Api.Domain.SharedKernel.Abstractions;

namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Argument guards. Each failure throws <see cref="DomainException"/> with a <c>Guard.*</c> validation error.</summary>
public static class Guard
{
    public static T AgainstNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : class =>
        value ?? throw Fail("Guard.Null", $"'{name}' must not be null.");

    public static string AgainstNullOrWhiteSpace(string? value, [CallerArgumentExpression(nameof(value))] string? name = null) =>
        string.IsNullOrWhiteSpace(value)
            ? throw Fail("Guard.NullOrWhiteSpace", $"'{name}' must not be null, empty or whitespace.")
            : value;

    public static T AgainstOutOfRange<T>(T value, T min, T max, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : IComparable<T> =>
        value.CompareTo(min) < 0 || value.CompareTo(max) > 0
            ? throw Fail("Guard.OutOfRange", $"'{name}' must be between {min} and {max} (was {value}).")
            : value;

    public static T AgainstNegative<T>(T value, [CallerArgumentExpression(nameof(value))] string? name = null)
        where T : INumberBase<T>, IComparisonOperators<T, T, bool> =>
        value < T.Zero
            ? throw Fail("Guard.Negative", $"'{name}' must not be negative (was {value}).")
            : value;

    public static TId AgainstDefault<TId>(TId id, [CallerArgumentExpression(nameof(id))] string? name = null)
        where TId : struct, IStronglyTypedId<Guid> =>
        id.Value == Guid.Empty
            ? throw Fail("Guard.Default", $"'{name}' must not be empty.")
            : id;

    private static DomainException Fail(string code, string message) =>
        new(new Error(code, message, ErrorType.Validation));
}
