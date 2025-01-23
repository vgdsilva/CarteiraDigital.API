using System.ComponentModel.DataAnnotations.Schema;

namespace CarteiraDigital.Domain.Entities;

[Table("user")]
public class User
{
    [Column("id")]
    public string Id { get; set; } = Guid.Empty.ToString();

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [NotMapped]
    public ICollection<Wallet> Wallets { get; set; }

    public User()
    {
        
    }
}
