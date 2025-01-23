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

    public async Task<Wallet?> GetWalletByUserIdAsync(string userId)
    {
        return await _context.Wallets.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
