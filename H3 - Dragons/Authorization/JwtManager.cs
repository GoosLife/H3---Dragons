using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace H3___Dragons.Authorization
{
    public static class JwtManager
    {
        static byte[] Key256Bit = new byte[32];

        static JwtManager()
        {
            // In this mockup example, the jwt manager just generates a 128 bit key and stores it in memory on startup. In the real world, this key would be stored in a secure location, such as environment variables, a database, etc.

            // Generate a 128 bit key randomly
            using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(Key256Bit);            
            }
        }

        public static string GenerateJwt(string dragonName, string role = "listener")
        {
            // Generate a JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey(); // Get the key from the environment variables, etc.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("dragonName", dragonName),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        public static bool ValidateToken(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();

            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                // Log the exception serverside to see what went wrong
                return false;
            }

            return true;
        }

        // Check if specific claim exists in JWT
        public static bool HasClaim(string jwt, string claimType, string claimValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = GetKey();
            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
                var claims = tokenHandler.ReadJwtToken(jwt).Claims;
                foreach (var claim in claims)
                {
                    if (claim.Type == claimType && claim.Value == claimValue)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                // Log the exception serverside to see what went wrong
                return false;
            }
            return false;
        }

        // Modify this method to retrieve the key from a secure location, such as environment variables, a database, etc.
        private static byte[] GetKey()
        {
            return Key256Bit;
        }
    }
}