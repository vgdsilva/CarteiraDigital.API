using CarteiraDigital.Domain.Models;

namespace CarteiraDigital.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Credentials credentials);
}
