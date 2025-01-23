namespace CarteiraDigital.Domain.Entities;

public class Wallet
{
    public string Id { get; set; } = Guid.Empty.ToString();

    public string UserId { get; set; } = Guid.Empty.ToString();

    public decimal Balance { get; set; } = 0;

}
