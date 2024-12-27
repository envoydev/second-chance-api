using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondChance.Domain.Common;

namespace SecondChance.Infrastructure.Persistence.Common;

internal class BaseAuditableEntityTypeConfiguration<TEntity> : BaseEntityTypeConfiguration<TEntity> where TEntity : BaseAuditableEntity
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.ChangedBy);
        builder.Property(x => x.DeletedAt);
        builder.Property(x => x.DeletedBy);
    }
}