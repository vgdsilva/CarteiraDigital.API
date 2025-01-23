using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infraestructure.Context;

public class PostgresSQLContext : DbContext
{

    public DbSet<User> Users { get; set; }

    public DbSet<Wallet> Wallets { get; set; }


    public PostgresSQLContext(DbContextOptions<PostgresSQLContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<User>().HasData([
            new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "root",
                    Username = "root",
                    Password = PasswordSecurity.HashPassword("root@123")
                }
            ]);
    }
}
