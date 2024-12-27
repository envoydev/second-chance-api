using Microsoft.EntityFrameworkCore;
using SecondChance.Infrastructure.Persistence;

namespace SecondChance.Infrastructure.Extensions;

internal static class ApplicationDbContextExtensions
{
    public static void SetDatabaseType(this DbContextOptionsBuilder<ApplicationDbContext> builder, string connectionString)
    {
        builder.UseSqlite(connectionString);
    }

    public static void SetDatabaseType(this DbContextOptionsBuilder builder, string connectionString)
    {
        builder.UseSqlite(connectionString);
    }
}