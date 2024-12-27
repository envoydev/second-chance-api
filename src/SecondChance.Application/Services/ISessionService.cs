using SecondChance.Domain.Enums;

namespace SecondChance.Application.Services;

public interface ISessionService
{
    Guid? UserId { get; }
    Role? UserRole { get; }
    public DateTime? TokenExpiration { get; }
}