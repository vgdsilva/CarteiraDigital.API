using CarteiraDigital.Application.Builders;
using CarteiraDigital.Domain.DTOs;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Models;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly WalletRepository _walletRepository;


        public WalletController(WalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        [HttpGet("{userId}/wallets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetUserWallets(string userId)
        {
            var wallets = await _walletRepository.GetByIdAsync(userId);
            return Ok(wallets);
        }

        [HttpPost("{userId}/wallets")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Wallet>> CreateUserWallet(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("userId is required").Build());

            var wallet = await _walletRepository.SaveChangesAsync(new Wallet() { UserId = userId});

            return CreatedAtAction(nameof(GetWalletById), new { wallet.Id }, wallet);
        }

        [HttpGet("{walletId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Wallet>> GetWalletById(string walletId)
        {
            if (string.IsNullOrEmpty(walletId))
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("walletId is required").Build());

            var wallet = await _walletRepository.GetByIdAsync(walletId);

            if (wallet is null)
                return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("Wallet is no found").Build());

            return Ok(wallet);
        }
        [HttpPost("{userId}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult>> AdicionarSaldo([FromBody] WalletDTO request)
        {
            try
            {
                if (request is null)
                    return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("request is required").Build());

                if (string.IsNullOrEmpty(request.Id))
                    return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("walletId is required").Build());
                
                if (request.Balance <= 0)
                    return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("balance must be grant then 0").Build());

                var wallet = await _walletRepository.GetByIdAsync(request.Id);

                if (wallet is null)
                    return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("Wallet is no found").Build());

                wallet.Balance += request.Balance;

                await _walletRepository.SaveChangesAsync(wallet);

                return Ok(new ApiResultBuilder().SetSuccess(true).SetData(wallet).Build());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetErrors(ex).Build());
            }
        }

        [HttpGet("{userId}/balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult>> ConsultarSaldo(string walletId)
        {
            try
            {
                if (string.IsNullOrEmpty(walletId))
                    return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("walletId is required").Build());

                var wallet = await _walletRepository.GetByIdAsync(walletId);

                if (wallet is null)
                    return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("Wallet is no found").Build());

                return Ok(new ApiResultBuilder().SetSuccess(true).SetData(new { balance = wallet.Balance }).Build());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetErrors(ex).Build());
            }
        }
    }
}
