namespace SecondChance.Application.CQRS.Transactions.Common;

internal interface ITransactionCurrency
{
    int? CurrencyCode { get; init; }
    string? CryptoCurrencyCode { get; init; }
}