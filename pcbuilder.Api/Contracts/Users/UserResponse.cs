using pcbuilder.Application.DTOs.Users;

namespace pcbuilder.Api.Contracts.Users;

public class UserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];

    public static UserResponse FromLoginDto(LoginDto loginDto)
    {
        return new UserResponse
        {
            Id = loginDto.Id,
            UserName = loginDto.UserName,
            AccessToken = loginDto.AccessToken,
            Roles = loginDto.Roles
        };
    }
}