using SecondChance.Domain.Enums;

namespace SecondChance.Application.Models.Settings;

public class SystemUserSettings
{
    public string UserName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public Role Role { get; init; }
}