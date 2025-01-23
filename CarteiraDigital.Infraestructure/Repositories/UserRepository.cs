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

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> SaveChangesAsync(User entity)
    {
        if (entity.Id == Guid.Empty.ToString())
        {
            if (await GetByUsernameAsync(entity.Username) != null)
                throw new Exception("Ja existe um usuario com este username.");
            
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
