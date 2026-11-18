namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

internal static class ValueObjectErrors
{
    internal static DomainException Invalid(string code, string message) =>
        new(new Error(code, message, ErrorType.Validation));
}
