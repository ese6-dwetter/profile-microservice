using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProfileMicroservice.Settings;

namespace ProfileMicroservice.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly TokenSettings _tokenSettings;

        public TokenGenerator(IOptions<TokenSettings> appSettings)
        {
            _tokenSettings = appSettings.Value;
        }

        public string GenerateJwt(Guid userId, string email, string username)
        {
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
            var issuer = _tokenSettings.Issuer;
            var audience = _tokenSettings.Audience;
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, username)
                }),
                Issuer = issuer,
                Audience = audience,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetJwtClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token.Replace("Bearer ", string.Empty)) as JwtSecurityToken;

            return securityToken?.Claims.First(claim => claim.Type == claimType).Value;
        }
    }
}
