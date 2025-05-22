using System.Security.Claims;

namespace MinimalMeet.Common.TokenHandler;

public interface ITokenReader
{
    Claim? ReadClaimFromToken(string token, string type);
}