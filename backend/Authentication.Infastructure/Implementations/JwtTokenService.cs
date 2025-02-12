using Application.Shared.Exceptions;
using Authentication.Application.Interfaces;
using Authentication.Application.Model.Token;
using Authentication.Domain.Entities;
using Authentication.Infastructure.Helpers.Jwt;
using CSharpFunctionalExtensions;
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

        public Result<TokenResponse> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var email = jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = jwtSecurityToken.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        
            if(email is null ||
                role is null)
            {
                return Result.Failure<TokenResponse>("Invalid Token");
            }

            return Result.Success(new TokenResponse
            {
                Email = email,
                Role = role,
            });
        }

        public string GenerateToken(User user)
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
                new Claim(ClaimTypes.Role, user.Role.RoleName)
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
