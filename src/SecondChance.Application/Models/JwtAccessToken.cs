using SecondChance.Domain.Enums;

namespace SecondChance.Application.Models;

public class JwtAccessToken
{
    public Guid UserId { get; init; }
    public Role Role { get; init; }
    public int IssuedAtUnixTimeStamp { get; init; }
    public int ExpiredAtUnixTimeStamp { get; init; }
}