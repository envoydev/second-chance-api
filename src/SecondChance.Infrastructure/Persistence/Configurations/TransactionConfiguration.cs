using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;
using SecondChance.Domain.Validations;
using SecondChance.Infrastructure.Persistence.Common;

namespace SecondChance.Infrastructure.Persistence.Configurations;

internal class TransactionConfiguration : BaseAuditableEntityTypeConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(nameof(IApplicationDbContext.Transactions));

        builder.Property(x => x.ProjectId).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(28, 2).IsRequired();
        builder.Property(x => x.CurrencyType).IsRequired();
        builder.Property(x => x.OperationType).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(TransactionValidations.DescriptionMaxLength);

        builder.HasOne(x => x.Project)
               .WithMany()
               .HasForeignKey(x => x.ProjectId)
               .OnDelete(DeleteBehavior.NoAction);

        base.Configure(builder);
    }
}