namespace SecondChance.Application.Models.Filters;

public interface IPagerFilter
{
    int? Skip { get; init; }
    int? Take { get; init; }
}