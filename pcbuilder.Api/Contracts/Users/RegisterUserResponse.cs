namespace pcbuilder.Api.Contracts.Users;

public class RegisterUserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
}