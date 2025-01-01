using FluentValidation;
using SecondChance.Application.CQRS.Transactions.Validators;
using SecondChance.Application.Persistant;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

// ReSharper disable once UnusedType.Global
internal sealed class GetAllTransactionsQueryValidator : AbstractValidator<GetAllTransactionsQuery>
{
    public GetAllTransactionsQueryValidator(IApplicationDbContext applicationDbContext)
    {
        RuleFor(v => v.ProjectId)
            .SetValidator(new NullableStructTypeValidator<Guid>(new TransactionProjectIdValidator(applicationDbContext)))
            .When(x => x.ProjectId.HasValue);
        
        Include(new DateRangeQueryFilterValidator());
        
        Include(new PagerFilterValidator());
    }
}