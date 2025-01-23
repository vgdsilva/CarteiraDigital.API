using CarteiraDigital.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public TransactionController()
        {
            
        }

        [HttpGet("GetTransactionsByUser/{id}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> TransactionsByUser(string userId)
        {


            return Ok();
        }
    }
}
