namespace Warehouse.Api.Domain.SharedKernel.ValueObjects;

/// <summary>Monetary amount in a currency.</summary>
public sealed record Money
{
    private Money(decimal amount, CurrencyCode currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public CurrencyCode Currency { get; }

    public static Money Create(decimal amount, CurrencyCode currency) => new(amount, Guard.AgainstNull(currency));

    public static Money Zero(CurrencyCode currency) => Create(0m, currency);

    public Money Add(Money other) => new(Amount + EnsureSameCurrency(other).Amount, Currency);

    public Money Subtract(Money other) => new(Amount - EnsureSameCurrency(other).Amount, Currency);

    public Money Multiply(decimal factor) => new(Amount * factor, Currency);

    private Money EnsureSameCurrency(Money other)
    {
        Guard.AgainstNull(other);
        return other.Currency == Currency
            ? other
            : throw ValueObjectErrors.Invalid("Common.CurrencyMismatch", $"Cannot combine amounts in '{Currency}' and '{other.Currency}'.");
    }
}
