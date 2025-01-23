using System.ComponentModel.DataAnnotations.Schema;

namespace CarteiraDigital.Domain.Entities
{
    [Table("transaction")]
    public class Transaction
    {
        [Column("id")]
        public string Id { get; set; } = Guid.Empty.ToString();

        [Column("senderwalletid")]
        public string SenderWalletId { get; set; } = Guid.Empty.ToString();

        [Column("receiverwalletid")]
        public string ReceiverWalletId { get; set; } = Guid.Empty.ToString();

        [Column("amount")]
        public decimal Amount { get; set; } = 0;

        [Column("transactiondate")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(name: nameof(ReceiverWalletId))]
        public virtual Wallet ReceiverWalletEntity { get; set; }

        [ForeignKey(name: nameof(SenderWalletId))]
        public virtual Wallet SenderWalletEntity { get; set; }
    }
}
