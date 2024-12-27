using FluentValidation;
using SecondChance.Application.Validators;

namespace SecondChance.Application.CQRS.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryValidator : AbstractValidator<GetAllProjectsQuery>
{
    public GetAllProjectsQueryValidator()
    {
        Include(new DateRangeFilterValidator());
        
        Include(new PagerFilterValidator());
    }
}