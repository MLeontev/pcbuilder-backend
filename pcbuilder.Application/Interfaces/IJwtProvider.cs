using pcbuilder.Domain.Models;

namespace pcbuilder.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateAccessToken(User user, IList<string> roles);

    string GenerateRefreshToken();
}