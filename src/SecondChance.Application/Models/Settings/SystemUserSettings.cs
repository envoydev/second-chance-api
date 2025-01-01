using SecondChance.Domain.Enums;

namespace SecondChance.Application.Models.Settings;

public class SystemUserSettings
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Role Role { get; set; }
}