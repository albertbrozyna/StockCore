namespace Warehouse.Api.Domain.SharedKernel;

/// <summary>Outcome of an operation without a value. Used in the Application layer.</summary>
public readonly struct Result
{
    private readonly Error? _error;

    private Result(Error error)
    {
        _error = error;
    }

    public bool IsSuccess => _error is null;

    public bool IsFailure => !IsSuccess;

    public Error Error => _error ?? Error.None;

    public static Result Success() => default;

    public static Result Failure(Error error) => new(Guard.AgainstNull(error));

    public static Result<T> Success<T>(T value) => Result<T>.Success(value);

    public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}
