using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;
using SecondChance.Domain.Validations;
using SecondChance.Infrastructure.Persistence.Common;

namespace SecondChance.Infrastructure.Persistence.Configurations;

internal class ProjectConfiguration : BaseAuditableEntityTypeConfiguration<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(IApplicationDbContext.Projects));

        builder.Property(x => x.Name).HasMaxLength(ProjectValidations.NameMaxLength);

        base.Configure(builder);
    }
}