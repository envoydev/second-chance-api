using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Transactions.Dtos;

public class TransactionResult
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public OperationType OperationType { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ChangedAt { get; set; }
}