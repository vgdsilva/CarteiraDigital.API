using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infraestructure.Repositories;

public class UserRepository
{
    private readonly PostgresSQLContext _context;

    public UserRepository(PostgresSQLContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }
}
