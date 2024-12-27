using SecondChance.Domain.Enums;

namespace SecondChance.Application.Security;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute
{
    private AuthorizeAttribute()
    {
    }

    public AuthorizeAttribute(Role[] role) : this()
    {
        Roles = role;
    }

    public Role[] Roles { get; private set; } = [];
}