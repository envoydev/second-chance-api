namespace SecondChance.Application.Models.QueryFilters;

public interface IPagerQueryFilter
{
    int? Skip { get; init; }
    int? Take { get; init; }
}