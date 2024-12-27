using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SecondChance.Application.Services;
using SecondChance.Domain.Common;

namespace SecondChance.Infrastructure.Persistence.Interceptors;

internal class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IDateTimeService _dateTimeService;
    private readonly ISessionService _sessionService;

    public AuditableEntityInterceptor(IDateTimeService dateTimeService,
        ISessionService sessionService)
    {
        _dateTimeService = dateTimeService;
        _sessionService = sessionService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = _dateTimeService.GetUtc();
                    entry.Entity.CreatedBy = _sessionService.UserId;

                    continue;
                case EntityState.Modified:
                    entry.Entity.ChangedAt = _dateTimeService.GetUtc();
                    entry.Entity.ChangedBy = _sessionService.UserId;

                    continue;
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = _dateTimeService.GetUtc();
                    entry.Entity.DeletedBy = _sessionService.UserId;
                    entry.State = EntityState.Modified;

                    continue;
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    continue;
            }
        }
    }
}