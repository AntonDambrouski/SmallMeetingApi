using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MinimalMeet.Common.TokenHandler;

public class TokenReader : ITokenReader
{
    public Claim? ReadClaimFromToken(string token, string type)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(type)) return null;

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        if (handler.CanReadToken(token))
        {
            var jwt = handler.ReadJwtToken(token);
            return jwt.Claims.FirstOrDefault(c => c.Type == type);
        }

        return null;
    }
}