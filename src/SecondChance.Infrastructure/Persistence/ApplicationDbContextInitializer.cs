using Microsoft.EntityFrameworkCore;
using SecondChance.Application.Logger;
using SecondChance.Application.Persistant;
using SecondChance.Application.Services;
using SecondChance.Domain.Entities;

namespace SecondChance.Infrastructure.Persistence;

internal class ApplicationDbContextInitializer : IApplicationDbContextInitializer
{
    private readonly IApplicationLogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly ISettingsService _settingsService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPasswordService _passwordService;

    public ApplicationDbContextInitializer(IApplicationLogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        ISettingsService settingsService,
        IPasswordService passwordService,
        IDateTimeService dateTimeService)
    {
        _logger = logger;
        _context = context;
        _settingsService = settingsService;
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

        var applicationSettings = _settingsService.GetSettings();
        var passwordHash = _passwordService.HashPassword(applicationSettings.SystemUser.Password);
        
        await _context.Users.AddAsync(new User
        {
            UserName = applicationSettings.SystemUser.UserName,
            Email = applicationSettings.SystemUser.Email,
            PasswordHash = passwordHash,
            Role = applicationSettings.SystemUser.Role,
            CreatedBy = null,
            CreatedAt = _dateTimeService.GetUtc()
        });

        await _context.SaveChangesAsync();
    }
}