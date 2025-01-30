using System.Security.Claims;
using pcbuilder.Domain.Models.Common;

namespace pcbuilder.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateAccessToken(User user, IList<string> roles);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}