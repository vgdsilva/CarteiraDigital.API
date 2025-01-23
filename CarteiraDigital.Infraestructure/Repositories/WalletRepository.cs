using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infraestructure.Repositories;

public class WalletRepository
{
    private readonly PostgresSQLContext _context;

    public WalletRepository(PostgresSQLContext context)
    {
        _context = context;
    }

    public async Task<List<Wallet>> GetListByUserIdAsync(string userId)
    {
        return await _context.Wallets.Where(x => x.UserId == userId).ToListAsync();
    }
    
    public async Task<Wallet?> GetByIdAsync(string walletId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(x => x.Id == walletId);
    }

    public async Task<Wallet> SaveChangesAsync(Wallet entity)
    {
        if (entity.Id == Guid.Empty.ToString())
        {
            entity.Id = Guid.NewGuid().ToString();

            _context.Wallets.Add(entity);
        }
        else
        {
            _context.Wallets.Update(entity);
        }

        await _context.SaveChangesAsync();

        return entity;
    }
}
