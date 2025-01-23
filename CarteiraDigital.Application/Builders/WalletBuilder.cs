using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Application.Builders;

public class WalletBuilder
{

    private readonly Wallet _wallet;

    public WalletBuilder()
    {
        _wallet = new Wallet();
    }

    public WalletBuilder SetUserId(string userId)
    {
        _wallet.UserId = userId;
        return this;
    }

    public WalletBuilder SetBalance(decimal balance)
    {
        _wallet.Balance = balance;
        return this;
    }

    public Wallet Build() => _wallet;
}
