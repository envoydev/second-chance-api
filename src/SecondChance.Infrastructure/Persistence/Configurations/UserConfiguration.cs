using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecondChance.Application.Persistant;
using SecondChance.Domain.Entities;
using SecondChance.Domain.Enums;
using SecondChance.Domain.Validations;
using SecondChance.Infrastructure.Persistence.Common;

namespace SecondChance.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : BaseAuditableEntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(IApplicationDbContext.Users));

        builder.Property(x => x.FirstName).HasMaxLength(UserValidations.FirstNameMaxLength);
        builder.Property(x => x.LastName).HasMaxLength(UserValidations.LastNameMaxLength);
        builder.Property(x => x.UserName).HasMaxLength(UserValidations.UserNameMaxLength).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(UserValidations.EmailMaxLength).IsRequired();
        builder.Property(x => x.Role).IsRequired().HasDefaultValue(UserValidations.DefaultRole);
        builder.Property(x => x.PasswordHash).HasMaxLength(UserValidations.PasswordHashMaxLength).IsRequired();
        builder.Property(x => x.RefreshToken).HasMaxLength(UserValidations.TokenRefreshMaxLength);
        builder.Property(x => x.RefreshTokenExpiration);

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.UserName).IsUnique();

        base.Configure(builder);
    }
}