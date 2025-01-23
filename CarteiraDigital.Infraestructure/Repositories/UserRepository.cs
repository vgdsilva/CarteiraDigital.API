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

    public async Task<User?> GetById(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> SaveAsync(User entity)
    {
        if (entity.Id == Guid.Empty.ToString())
        {
            entity.Id = Guid.NewGuid().ToString();

            _context.Users.Add(entity);
        }
        else
        {
            _context.Users.Update(entity);
        }

        await _context.SaveChangesAsync();

        return entity;
    }
}
