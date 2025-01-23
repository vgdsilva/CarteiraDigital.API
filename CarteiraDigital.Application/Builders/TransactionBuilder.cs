using CarteiraDigital.Application.Interfaces;
using CarteiraDigital.Domain.Entities;

namespace CarteiraDigital.Application.Builders
{
    public class TransactionBuilder : IBuilder<Transaction>
    {
        private readonly Transaction _transaction;
        public TransactionBuilder()
        {
            _transaction = new Transaction();
        }

        public TransactionBuilder SetAmount(int amount)
        {
            _transaction.Amount = amount;
            return this;
        }

        public TransactionBuilder SetSenderId(string senderId)
        {
            _transaction.SenderId = senderId;
            return this;
        }

        public TransactionBuilder SetReceiverId(string receiverId)
        {
            _transaction.ReceiverId = receiverId;
            return this;
        }

        public Transaction Build() => _transaction;
    }
}
