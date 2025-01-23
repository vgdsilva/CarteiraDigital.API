using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CarteiraDigital.Domain.DTOs;

public class TransactionDTO
{

    [SwaggerSchema(Description = "Chave Primária da carteira que esta realizando a transação")]
    [Required(ErrorMessage = "SenderWalletId é obrigatório")]
    public string SenderWalletId { get; set; }

    [SwaggerSchema(Description = "Chave Primária da carteira que esta enviado a transação")]
    [Required(ErrorMessage = "ReceiverWalletId é obrigatório")]
    public string ReceiverWalletId { get; set; }

    [SwaggerSchema(Description = "Valor da transação")]
    [Required(ErrorMessage = "Amount é obrigatório")]
    public decimal Amount { get; set; }
}
