using pcbuilder.Application.DTOs.Users;

namespace pcbuilder.Api.Contracts.Users;

public class AuthResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];

    public static AuthResponse FromAuthResult(AuthResult authResult)
    {
        return new AuthResponse
        {
            Id = authResult.Id,
            UserName = authResult.UserName,
            AccessToken = authResult.AccessToken,
            Roles = authResult.Roles
        };
    }
}