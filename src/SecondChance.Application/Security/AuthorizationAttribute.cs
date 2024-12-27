using SecondChance.Domain.Enums;

namespace SecondChance.Application.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizationAttribute : Attribute
{
    private AuthorizationAttribute()
    {
    }

    public AuthorizationAttribute(Role[] role) : this()
    {
        Roles = role;
    }

    public Role[] Roles { get; private set; } = [];
}