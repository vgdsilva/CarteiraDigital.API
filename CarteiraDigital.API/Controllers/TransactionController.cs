using CarteiraDigital.Application.Builders;
using CarteiraDigital.Domain.DTOs;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Models;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseApiController
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly WalletRepository _walletRepository;


        public TransactionController(TransactionRepository transactionRepository, WalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Transaction>>> TransactionsByUser(string userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            try
            {
                var wallets = await _walletRepository.GetListByUserIdAsync(userId);

                var list = await _transactionRepository.GetListByWalletIdAsync(walletIds: wallets.Select(x => x.Id).ToArray(), startDate, endDate);

                return Ok(new ApiResultBuilder().SetSuccess(true).SetData(list).Build());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetErrors(ex).Build());
            }
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult>> CreateTransaction([FromBody] TransactionDTO request)
        {
            try
            {
                if (request is null)
                    return BadRequest(new ApiResultBuilder().SetMessage("request is required").SetSuccess(false).Build());

                if (request.Amount <= 0)
                    return BadRequest(new ApiResultBuilder().SetMessage("amount must be grate then 0").SetSuccess(false).Build());

                if (string.IsNullOrEmpty(request.SenderWalletId))
                    return BadRequest(new ApiResultBuilder().SetMessage("sender id is required").SetSuccess(false).Build());

                if (string.IsNullOrEmpty(request.ReceiverWalletId))
                    return BadRequest(new ApiResultBuilder().SetMessage("receiver id is required").SetSuccess(false).Build());

                var senderWalletEntity = await _walletRepository.GetByIdAsync(request.SenderWalletId);
                var receiverWalletEntity = await _walletRepository.GetByIdAsync(request.ReceiverWalletId);

                // Verifique se as carteiras existem e se o remetente tem saldo suficiente
                if (senderWalletEntity == null || receiverWalletEntity == null)
                    return BadRequest(new ApiResultBuilder().SetMessage("Carteira não encontrada.").SetSuccess(false).Build());
                
                if (senderWalletEntity.Balance < request.Amount)
                    return BadRequest(new ApiResultBuilder().SetMessage("Saldo insuficiente na carteira.").SetSuccess(false).Build());

                senderWalletEntity.Balance -= request.Amount;
                receiverWalletEntity.Balance += request.Amount;

                var entity = Mapper.Map<Transaction>(request);

                await _transactionRepository.SaveChangesAsync(entity);
                await _walletRepository.SaveChangesAsync(senderWalletEntity);
                await _walletRepository.SaveChangesAsync(receiverWalletEntity);

                return Ok(new ApiResultBuilder().SetSuccess(true).SetData(entity).Build());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetErrors(ex).Build());
            }
        }
    }
}
