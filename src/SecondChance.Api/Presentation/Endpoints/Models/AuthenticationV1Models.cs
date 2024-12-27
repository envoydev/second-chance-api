namespace SecondChance.Api.Presentation.Endpoints.Models;

public record LoginV1Body(string UserName, string Password);

public record RefreshV1Body(string AccessToken, string RefreshToken);