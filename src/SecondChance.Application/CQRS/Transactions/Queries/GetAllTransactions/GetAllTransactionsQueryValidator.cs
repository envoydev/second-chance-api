using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Errors;
using SecondChance.Application.Persistant;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Transactions.Queries.GetAllTransactions;

public class GetAllTransactionsQueryValidator : AbstractValidator<GetAllTransactionsQuery>
{
    private readonly IApplicationDbContext _applicationDbContext;
    
    public GetAllTransactionsQueryValidator(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;

        RuleFor(v => v.ProjectId)
            .NotEmpty()
            .WithErrorCode(ErrorMessageCodes.ValidationInvalidValue)
            .MustAsync(CheckIsProjectExistAsync)
            .WithErrorCode(ErrorMessageCodes.ValidationRecordNotFound)
            .When(x => x.ProjectId.HasValue);
        
        Include(new DateRangeFilterValidator());
        
        Include(new PagerFilterValidator());
    }

    private Task<bool> CheckIsProjectExistAsync(GetAllTransactionsQuery qetAllTransactionsQuery, Guid? projectId, CancellationToken cancellationToken)
    {
        return _applicationDbContext.Projects.AsNoTracking().AnyAsync(x => x.Id == projectId!, cancellationToken);
    }
}