namespace pcbuilder.Infrastructure.Authentication;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;

    public int AccessTokenExpiryInMinutes { get; set; }
}