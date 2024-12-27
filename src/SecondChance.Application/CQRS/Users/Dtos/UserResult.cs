using SecondChance.Domain.Enums;

namespace SecondChance.Application.CQRS.Users.Dtos;

public class UserResult
{
    public Guid Id { get; init; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}