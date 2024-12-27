namespace SecondChance.Application.Models.Filters;

public interface IDateRangeFilter
{
    DateTime? From { get; init; }
    DateTime? To { get; init; }
}