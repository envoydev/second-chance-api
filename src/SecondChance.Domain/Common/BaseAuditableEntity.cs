namespace SecondChance.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Guid? ChangedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}