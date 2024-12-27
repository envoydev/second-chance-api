using Microsoft.AspNetCore.Identity;
using SecondChance.Application.Logger;
using SecondChance.Application.Services;

namespace SecondChance.Infrastructure.Services;

internal class PasswordService : IPasswordService
{
    private readonly IApplicationLogger<PasswordService> _logger;
    private readonly PasswordHasher<object> _passwordHasher;
    private readonly object _user;

    public PasswordService(IApplicationLogger<PasswordService> logger)
    {
        _logger = logger;
        _user = new object();
        _passwordHasher = new PasswordHasher<object>();
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(_user, password);
    }

    public bool Verify(string password, string hashPassword)
    {
        try
        {
            var result = _passwordHasher.VerifyHashedPassword(_user, hashPassword, password);

            return result == PasswordVerificationResult.Success;
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Cannot verify password and hash password");

            return false;
        }
    }
}