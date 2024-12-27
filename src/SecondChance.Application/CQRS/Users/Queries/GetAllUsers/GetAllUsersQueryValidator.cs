using FluentValidation;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        Include(new DateRangeFilterValidator());
        
        Include(new PagerFilterValidator());
    }
}