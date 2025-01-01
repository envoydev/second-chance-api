namespace SecondChance.Application.Models.QueryFilters;

public interface IDateRangeQueryFilter
{
    DateTime? From { get; init; }
    DateTime? To { get; init; }
}