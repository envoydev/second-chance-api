namespace SecondChance.Application.Models.Settings;

public class ApplicationSettings
{
    public ConnectionStringsSettings ConnectionStrings { get; init; } = null!;
    public JwtConfigSettings JwtConfig { get; init; } = null!;
    public SystemUserSettings SystemUser { get; init; } = null!;
    public RequestConfigSettings RequestConfig { get; init; } = null!;
}