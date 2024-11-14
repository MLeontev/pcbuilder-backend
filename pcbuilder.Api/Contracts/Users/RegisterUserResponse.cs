using pcbuilder.Application.DTOs.Users;

namespace pcbuilder.Api.Contracts.Users;

public class RegisterUserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];

    public static RegisterUserResponse FromLoginDto(LoginDto loginDto)
    {
        return new RegisterUserResponse
        {
            Id = loginDto.Id,
            UserName = loginDto.UserName,
            AccessToken = loginDto.AccessToken,
            Roles = loginDto.Roles
        };
    }
}