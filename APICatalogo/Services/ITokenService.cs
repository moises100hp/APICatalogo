using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services
{
    /// <inheritdoc/>
    public interface ITokenService
    {
        /// <inheritdoc/>
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims,
                                             IConfiguration _config);
        /// <inheritdoc/>
        string GenerateRefreshToken();

        /// <inheritdoc/>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token,
                                                     IConfiguration _config);
    }
}
