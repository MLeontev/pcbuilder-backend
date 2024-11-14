namespace pcbuilder.Application.DTOs.Users;

public class LoginDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
}