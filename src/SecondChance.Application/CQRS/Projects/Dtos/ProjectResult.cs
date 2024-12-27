namespace SecondChance.Application.CQRS.Projects.Dtos;

public sealed class ProjectResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}