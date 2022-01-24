using ISSProjectFINAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ISSProjectFINAL
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom secret key for authentication");
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new System.Security.Claims.ClaimsIdentity(GetClaims(user)),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static Claim[] GetClaims(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(GetUnclassifiedClaim(user));
            claims.Add(GetConfidentialClaim(user));
            claims.Add(GetRestrictedClaim(user));
            claims.Add(GetTopSecretClaim(user));

            return claims.Where(x => x != null).ToArray();
        }

        private static Claim? GetTopSecretClaim(User user)
        {
            if (user.AccessLevel != "TopSecret") return null;
            return new Claim(ClaimTypes.Role, "TopSecret");
        }

        private static Claim? GetConfidentialClaim(User user)
        {
            if (user.AccessLevel != "TopSecret" &&
                user.AccessLevel != "Confidential") return null;
            return new Claim(ClaimTypes.Role, "Confidential");
        }

        private static Claim? GetRestrictedClaim(User user)
        {
            if (user.AccessLevel != "TopSecret" &&
                user.AccessLevel != "Confidential" &&
                user.AccessLevel != "Restricted") return null;
            return new Claim(ClaimTypes.Role, "Restricted");
        }

        private static Claim? GetUnclassifiedClaim(User user)
        {
            if (user.AccessLevel != "TopSecret" &&
                user.AccessLevel != "Confidential" &&
                user.AccessLevel != "Restricted" &&
                user.AccessLevel != "Unclassified") return null;
            return new Claim(ClaimTypes.Role, "Unclassified");
        }
    }
}