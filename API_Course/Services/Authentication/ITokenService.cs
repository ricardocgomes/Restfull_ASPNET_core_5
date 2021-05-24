using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace Mvc.Services.Authentication
{
    public interface ITokenService
    {
        string GenerateAcessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpireToken(string token);
    }
}
