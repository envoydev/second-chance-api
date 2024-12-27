using SecondChance.Domain.Enums;

namespace SecondChance.Domain.Validations;

public abstract class UserValidations
{
    private const string NameCharactersValidation = @"^[\p{L}\p{M}'`\-]+$";
    private static readonly string[] AllowedCharactersForName = ["a-Z", "-", "`", "'"];

    public const int EmailMinLength = 8;
    public const int EmailMaxLength = 128;
    public const int PasswordHashMaxLength = 256;
    public const int TokenRefreshMaxLength = 1024;
    public const Role DefaultRole = Role.User;
    public const string UserNameCharactersValidation = @"^[a-zA-Z0-9_\-]+$";
    public static readonly string[] UserNameAllowedCharacters = ["a-Z",  "0-9", "_", "-"];
    public const int UserNameMinLength = 2;
    public const int UserNameMaxLength = 128;
    public const string FirstNameCharactersValidation = NameCharactersValidation;
    public static readonly string[] FirstNameAllowedCharacters = AllowedCharactersForName;
    public const int FirstNameMinLength = 1;
    public const int FirstNameMaxLength = 50;
    public const string LastNameCharactersValidation = NameCharactersValidation;
    public static readonly string[] LastNameAllowedCharacters = AllowedCharactersForName;
    public const int LastNameMinLength = 1;
    public const int LastNameMaxLength = 50;
}