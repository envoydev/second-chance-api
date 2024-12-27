using SecondChance.Domain.Common;
using SecondChance.Domain.Enums;

namespace SecondChance.Domain.Entities;

public class Transaction : BaseAuditableEntity
{
    public Guid ProjectId { get; set; }
    public OperationType OperationType { get; set; }
    public CurrencyType CurrencyType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public Project Project { get; set; } = null!;
}