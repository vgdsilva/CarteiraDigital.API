using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteiraDigital.Domain.DTOs
{
    public class WalletDTO
    {
        [SwaggerSchema(Description = "Chave Primária da carteira")]
        [Required(ErrorMessage = "Id é obrigatório")]
        public string Id { get; set; }

        [SwaggerSchema(Description = "Valor do Saldo")]
        [Required(ErrorMessage = "Amount é obrigatório")]
        public decimal Balance { get; set; }
    }
}
