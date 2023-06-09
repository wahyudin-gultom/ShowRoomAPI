using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShowRoomAPI.Const;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShowRoomAPI
{
    public class JwtBearerManager: IJwtBearerManager
    {

        private readonly JwtConfig _config;

        public JwtBearerManager(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        public string GenerateToken()
        {
            var symmetricSecurityKey = Encoding.ASCII.GetBytes(_config.Secret);
            var handler = new JwtSecurityTokenHandler();

            var desc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("username",UserData.UserName),
                    new Claim(ClaimTypes.Role, "Sales")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_config.DurationInMinute),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricSecurityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(desc);

            var tokenString = handler.WriteToken(token);
            return tokenString;
        }

        public ClaimsPrincipal GetAuthTokenResult(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token);

            if(jwtToken == null) return null;

            var symmetricSecurityKey = Encoding.ASCII.GetBytes(_config.Secret);

            var claimPrincipal = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(symmetricSecurityKey)
            };

            var claim = handler.ValidateToken(token, claimPrincipal, out _);

            return claim;
        }
    }
}
