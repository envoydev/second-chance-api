using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;

namespace SecondChance.Infrastructure.Persistence;

internal class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private IDbContextTransaction? _currentTransaction;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Project> Projects => Set<Project>();

    public async Task StartTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
        }

        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            return;
        }

        await _currentTransaction.CommitAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
        {
            return;
        }

        await _currentTransaction.RollbackAsync(cancellationToken);
    }

    public new Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }

    #region Overrided methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<User>().HasQueryFilter(o => o.DeletedAt == null);
        modelBuilder.Entity<Project>().HasQueryFilter(o => o.DeletedAt == null);
        modelBuilder.Entity<Transaction>().HasQueryFilter(o => o.DeletedAt == null);
    }

    #endregion
}