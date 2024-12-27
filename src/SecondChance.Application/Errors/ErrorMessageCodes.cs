namespace SecondChance.Application.Errors;

public abstract class ErrorMessageCodes
{
    // Validation Specific Codes
    public const string ValidationWrongUserNameOrPassword = "VALIDATION_WRONG_USERNAME_OR_PASSWORD";
    public const string ValidationRequiredValue = "VALIDATION_REQUIRED_VALUE";
    public const string ValidationInvalidValue = "VALIDATION_INVALID_VALUE";
    public const string ValidationMaxLengthExceeded = "VALIDATION_MAX_LENGTH_EXCEEDED";
    public const string ValidationInvalidCharacters = "VALIDATION_INVALID_CHARACTERS";
    public const string ValidationInvalidRange = "VALIDATION_INVALID_RANGE";
    public const string ValidationSameValueExist = "VALIDATION_SAME_VALUE_EXIST";
    public const string ValidationRecordNotFound = "VALIDATION_RECORD_NOT_FOUND";
    public const string ValidationActionIsNotAllowed = "VALIDATION_ACTION_IS_NOT_ALLOWED";

    // Token-Specific Codes
    public const string TokenRequired = "TOKEN_REQUIRED";
    public const string TokenInvalid = "TOKEN_INVALID";
    public const string TokenExpired = "TOKEN_EXPIRED";

    // Exception Handling Specific Codes
    public const string InternalServerError = "EXCEPTION_INTERNAL_SERVER";
    public const string UnauthorizedAccess = "EXCEPTION_UNAUTHORIZED";
    public const string ForbiddenAccess = "EXCEPTION_FORBIDDEN";
    public const string NotFound = "EXCEPTION_NOT_FOUND";
    public const string BadRequest = "EXCEPTION_BAD_REQUEST";
}