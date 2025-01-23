using CarteiraDigital.Application.Builders;
using CarteiraDigital.Domain.Models;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly WalletRepository _walletRepository;


        public WalletController(WalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet("GetBalance/{userId}")]
        public async Task<ActionResult<ApiResult>> ConsultarSaldo(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("userId is required").Build());

                var userWallet = await _walletRepository.GetWalletByUserIdAsync(userId);

                if (userWallet is null)
                    return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("Wallet was not found").Build());

                return Ok(new ApiResultBuilder().SetSuccess(true).SetData(userWallet.Balance).Build());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetErrors(ex).Build());
            }
        }
    }
}
