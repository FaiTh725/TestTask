using Application.Shared.Exceptions;
using Authentication.Application.Interfaces;
using Authentication.Application.Model.User;
using Authentication.Infastructure.Helpers.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Infastructure.Implementations
{
    public class JwtTokenService : IAuthTokenService
    {
        private readonly IConfiguration configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(UserResponse user)
        {
            var jwtSetting = configuration
                .GetSection("JwtSetting")
                .Get<JwtConf>() ??
                throw new AplicationConfigurationException("Jwt Setting");

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                audience: jwtSetting.Audience,
                issuer: jwtSetting.Issuer,
                expires: DateTime.UtcNow.AddMinutes(15)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
