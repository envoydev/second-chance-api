using FluentValidation;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

internal sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        Include(new DateRangeQueryFilterValidator());
        
        Include(new PagerFilterValidator());
    }
}