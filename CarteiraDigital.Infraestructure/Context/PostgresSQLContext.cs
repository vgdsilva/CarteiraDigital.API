using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CarteiraDigital.Infraestructure.Context;

public class PostgresSQLContext : DbContext
{

    public DbSet<User> Users { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

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

        modelBuilder.Entity<User>();
        modelBuilder.Entity<Transaction>();

        _ = modelBuilder.Entity<User>().HasData(
            [
                new User
                {
                    Id = "aa73b66b-4ff9-4d9f-98b5-59cc3752a0a8",
                    Name = "User01",
                    Username = "user01",
                    Password = PasswordSecurity.HashPassword("123")
                },
                new User
                {
                    Id = "500e1036-6d49-44a5-9610-3a9525f617ea",
                    Name = "User02",
                    Username = "user02",
                    Password = PasswordSecurity.HashPassword("123")
                }
            ]);

        _ = modelBuilder.Entity<Wallet>().HasData(
            [
                new Wallet
                {
                    Id = "c5758012-68bb-4da7-be1a-67558066e36b",
                    UserId = "aa73b66b-4ff9-4d9f-98b5-59cc3752a0a8",
                    Balance = 1000,
                },
                new Wallet
                {
                    Id = "091f75da-0fd5-4cf9-a3ed-3f3eca546a0d",
                    UserId = "500e1036-6d49-44a5-9610-3a9525f617ea",
                    Balance = 500
                },
            ]);
    }
}
