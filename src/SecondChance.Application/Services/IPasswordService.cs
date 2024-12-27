namespace SecondChance.Application.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool Verify(string password, string hashPassword);
}