using FluentValidation;
using SecondChance.Application.CQRS.Transactions.Common;
using SecondChance.Application.Errors;

namespace SecondChance.Application.CQRS.Transactions.Validators;

internal class TransactionCurrencyValidator : AbstractValidator<ITransactionCurrency>
{
    public TransactionCurrencyValidator()
    {
        RuleFor(v => v.CurrencyCode)
            .MustAsync(CheckIsCurrencyCodeOrCryptoCurrencyCodeExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredOneOfValue);
        
        RuleFor(v => v.CryptoCurrencyCode)
            .MustAsync(CheckIsCurrencyCodeOrCryptoCurrencyCodeExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRequiredOneOfValue);
    }

    private static Task<bool> CheckIsCurrencyCodeOrCryptoCurrencyCodeExistAsync(ITransactionCurrency transactionCurrency, int? currencyCode, CancellationToken cancellationToken)
    {
        return Task.FromResult(currencyCode.HasValue && transactionCurrency.CurrencyCode.HasValue);
    }
    
    private static Task<bool> CheckIsCurrencyCodeOrCryptoCurrencyCodeExistAsync(ITransactionCurrency transactionCurrency, string? cryptoCurrencyCode, CancellationToken cancellationToken)
    {
        return Task.FromResult(cryptoCurrencyCode != null && transactionCurrency.CurrencyCode.HasValue);
    }
}