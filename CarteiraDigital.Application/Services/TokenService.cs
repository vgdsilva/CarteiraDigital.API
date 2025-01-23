using CarteiraDigital.Application.Interfaces;
using CarteiraDigital.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CarteiraDigital.Application.Services;

public class TokenService : ITokenService
{

    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Credentials credentials)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(/*_configuration["Jwt:Secret"]*/"MjItMDEtMjAyNSBlc3RlIGUgdW0gY29kaWdvIHNlY3JldG8gc3VwZXIgc2VjcmV0byAyMi0wMS0yMDI1IA==");
        
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            Subject = new ClaimsIdentity(
                    new GenericIdentity(credentials.Username, "Username")
                    //new[] {
                    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //    new Claim(JwtRegisteredClaimNames.UniqueName, credentials.Username),
                    //    }
                    ),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = "https://localhost:7263/",
            Audience = "https://localhost:7263/",
        });

        return tokenHandler.WriteToken(token);
    }
}
