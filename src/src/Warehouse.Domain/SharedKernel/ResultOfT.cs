namespace Warehouse.Domain.SharedKernel;

/// <summary>Outcome of an operation with a value. Used in the Application layer.</summary>
public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Error? _error;

    private Result(T? value, Error? error)
    {
        _value = value;
        _error = error;
    }

    public bool IsSuccess => _error is null;

    public bool IsFailure => !IsSuccess;

    public Error Error => _error ?? Error.None;

    /// <summary>The value of a successful result; throws <see cref="InvalidOperationException"/> on failure.</summary>
    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException($"Cannot read the value of a failed result ({Error.Code}).");

    internal static Result<T> Success(T value) => new(value, null);

    internal static Result<T> Failure(Error error) => new(default, Guard.AgainstNull(error));
}
