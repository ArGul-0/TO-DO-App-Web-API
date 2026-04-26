using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions options;

        public JwtTokenGenerator(IOptions<JwtOptions> options) // Constructor For JwtTokenGenerator, Accepting IConfiguration To Access JWT Settings From Configuration
        {
            this.options = options.Value;
        }

        /// <summary>
        /// Generates A JWT Access Token For The Specified User, Containing Claims For User ID And Username, And Signed With A Secret Key From Configuration.
        /// </summary>
        /// <param name="user">The User For Whom The Token Is Being Generated.</param>
        /// <returns>A JWT Access Token As A String.</returns>
        public string GenerateAccessToken(User user)
        {
            var secretKey = options.SecretKey ?? throw new Exception("JWT Secret Key is not configured."); // Retrieve The Secret Key From Configuration To Sign The JWT Token, Ensuring That It Is Properly Configured To Avoid Runtime Errors During Token Generation.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = options.Issuer ?? throw new Exception("JWT Issuer is not configured.");
            var audience = options.Audience ?? throw new Exception("JWT Audience is not configured.");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(options.ExpirationHours),
                SigningCredentials = credentials,
                Issuer = issuer,
                Audience = audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
