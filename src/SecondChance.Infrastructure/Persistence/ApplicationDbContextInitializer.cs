using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Logger;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;
using SecondChance.Domain.Entities;
using SecondChance.Domain.Enums;

namespace SecondChance.Infrastructure.Persistence;

public class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTimeService _dateTimeService;
    private readonly IApplicationLogger<ApplicationDbContextInitializer> _logger;
    private readonly IPasswordService _passwordService;

    public ApplicationDbContextInitializer(IApplicationLogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        IPasswordService passwordService,
        IDateTimeService dateTimeService)
    {
        _logger = logger;
        _context = context;
        _passwordService = passwordService;
        _dateTimeService = dateTimeService;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while initialising the database.");

            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while seeding the database.");

            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        if (_context.Users.IgnoreQueryFilters().Any())
        {
            return;
        }

        await _context.Users.AddAsync(new User
        {
            UserName = "super_admin",
            Email = "admin@second-chance.io",
            PasswordHash = _passwordService.HashPassword("Soul1234!"),
            Role = Role.SuperAdmin,
            CreatedBy = null,
            CreatedAt = _dateTimeService.GetUtc()
        });

        await _context.SaveChangesAsync();
    }
}