using Microsoft.AspNetCore.Mvc;
using SecondChance.Application.Models.Filters;

namespace SecondChance.Api.Presentation.Endpoints.Models;

public record GetUsersV1QueryParams(
    [FromQuery(Name = "skip")] int? Skip,
    [FromQuery(Name = "take")] int? Take,
    [FromQuery(Name = "from")] DateTime? From,
    [FromQuery(Name = "to")] DateTime? To) : IPagerFilter, IDateRangeFilter;