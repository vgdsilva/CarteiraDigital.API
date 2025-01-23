using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Infraestructure.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infraestructure.Repositories
{
    public class TransactionRepository
    {
        private readonly PostgresSQLContext _context;

        public TransactionRepository(PostgresSQLContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetListByWalletIdAsync(string[] walletIds, DateTime? startDate,DateTime? endDate)
        {
            return await _context.Transactions
                .Where(x => (walletIds.Any(w => w == x.SenderWalletId) || walletIds.Any(w => w == x.ReceiverWalletId))
                    && (startDate == null || x.TransactionDate >= startDate)
                    && (endDate == null || x.TransactionDate <= endDate))
                .ToListAsync();
        }

        public async Task<Transaction> SaveChangesAsync(Transaction entity)
        {
            if (entity.Id == Guid.Empty.ToString())
            {
                entity.Id = Guid.NewGuid().ToString();

                _context.Transactions.Add(entity);
            }
            else
            {
                _context.Transactions.Update(entity);
            }

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
