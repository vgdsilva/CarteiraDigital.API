using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteiraDigital.Domain.Models
{
    public class Token
    {
        public string AccessToken { get; set; } = string.Empty;
        public string UserId { get; set; } = Guid.Empty.ToString();
        public DateTime ExpireIn { get; set; } 
    }
}
