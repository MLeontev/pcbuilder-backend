namespace pcbuilder.Application.DTOs.Users;

public class UserRegistrationResult
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}