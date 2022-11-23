using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace GrandTripAPI.Data.Repositories
{
    public class JwtService
    {
        private readonly string _secret;

        public JwtService(IConfiguration cfg)
        {
            _secret = cfg.GetValue<string>("jwtsecret");
        }

        public string GenerateToken(int id)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []{new Claim("Id", $"{id}")}),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }

        public int? RetrieveId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out var validatedToken);
                
                return int.Parse((validatedToken as JwtSecurityToken)?.Claims.FirstOrDefault(
                    c => c.Type == "Id")?.Value ?? string.Empty);
            }
            catch
            {
                return null;
            }
        }
    }
}