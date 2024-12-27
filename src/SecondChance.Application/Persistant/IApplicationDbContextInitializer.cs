namespace SecondChance.Application.Persistant;

public interface IApplicationDbContextInitializer
{
    Task InitialiseAsync();
    Task SeedAsync();
}