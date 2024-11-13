using Microsoft.AspNetCore.Identity;

namespace pcbuilder.Domain.Models;

public class User : IdentityUser<int>
{
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime RefreshTokenExpiryTime { get; set; }
}