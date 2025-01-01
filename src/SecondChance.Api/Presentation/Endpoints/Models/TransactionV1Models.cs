using Microsoft.AspNetCore.Mvc;
using SecondChance.Application.Models.QueryFilters;
using SecondChance.Domain.Enums;

namespace SecondChance.Api.Presentation.Endpoints.Models;

public record GetTransactionsV1QueryParams(
    [FromQuery(Name = "projectId")] Guid? ProjectId, 
    [FromQuery(Name = "skip")] int? Skip,
    [FromQuery(Name = "take")] int? Take,
    [FromQuery(Name = "from")] DateTime? From,
    [FromQuery(Name = "to")] DateTime? To) : IPagerQueryFilter, IDateRangeQueryFilter;

public record CreateTransactionV1Body(
    Guid ProjectId,
    OperationType? OperationType,
    int? CurrencyCode,
    string? CryptoCurrencyCode,
    decimal? Amount,
    string? Description);

public record UpdateTransactionV1Body(
    Guid ProjectId,
    OperationType? OperationType,
    int? CurrencyCode,
    string? CryptoCurrencyCode,
    decimal? Amount,
    string? Description);
