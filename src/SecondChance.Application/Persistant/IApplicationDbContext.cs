using Microsoft.EntityFrameworkCore;
using SecondChance.Domain.Entities;

namespace SecondChance.Application.Persistant;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Project> Projects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task StartTransactionAsync();
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}