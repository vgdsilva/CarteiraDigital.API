using System.ComponentModel.DataAnnotations.Schema;

namespace CarteiraDigital.Domain.Entities;

[Table("wallet")]
public class Wallet
{
    [Column("id")]
    public string Id { get; set; } = Guid.Empty.ToString();

    [Column("userid")]
    public string UserId { get; set; } = Guid.Empty.ToString();

    [Column("balance")]
    public decimal Balance { get; set; } = 0;

    [ForeignKey(nameof(UserId))]
    public virtual User UserEntity { get; set; }

    [NotMapped]
    public ICollection<Transaction> Transactions { get; set; }
}
