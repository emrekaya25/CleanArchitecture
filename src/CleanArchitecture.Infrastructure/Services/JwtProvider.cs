using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            var expires = DateTime.Now.AddDays(1);

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
            SigningCredentials signingCredentials = new(securityKey,SecurityAlgorithms.HmacSha512);// security key ve algoritma ister

            List<Claim> claims = new() //tokenın içine yazılacak bilgiler
            {
                new Claim("user-id",user.Id.ToString())
            };

            JwtSecurityToken securityToken = new(
                issuer:options.Value.Issuer,
                audience:options.Value.Audience,
                claims:claims,
                notBefore:DateTime.Now,
                expires:expires,
                signingCredentials:signingCredentials);//Handler'a securityToken verdiğimizde token'ı üretiyor.

            JwtSecurityTokenHandler handler = new();

            string token = handler.WriteToken(securityToken);

            return Task.FromResult(token);
        }
    }
}
