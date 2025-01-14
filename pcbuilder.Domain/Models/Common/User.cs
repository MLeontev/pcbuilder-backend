using Microsoft.AspNetCore.Identity;

namespace pcbuilder.Domain.Models.Common;

public class User : IdentityUser<int>
{
    public string RefreshToken { get; set; } = string.Empty;

    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public List<Build> Builds { get; set; } = [];
}