using SecondChance.Domain.Common;

namespace SecondChance.Domain.Entities;

public class Project : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
}