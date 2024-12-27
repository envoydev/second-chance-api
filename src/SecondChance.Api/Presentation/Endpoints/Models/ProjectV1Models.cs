using Microsoft.AspNetCore.Mvc;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Api.Presentation.Endpoints.Models;

public record GetProjectsV1QueryParams(
    [FromQuery(Name = "skip")] int? Skip,
    [FromQuery(Name = "take")] int? Take,
    [FromQuery(Name = "from")] DateTime? From,
    [FromQuery(Name = "to")] DateTime? To) : IPagerFilter, IDateRangeFilter;

public record CreateProjectV1Body(string Name);

public record UpdateProjectV1Body(string Name);