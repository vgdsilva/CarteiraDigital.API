using CarteiraDigital.Application.Builders;
using CarteiraDigital.Application.Interfaces;
using CarteiraDigital.Domain.Models;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult>> GetById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("userId is required").Build());

            Domain.Entities.User? user = await _userRepository.GetByIdAsync(userId);

            if (user is null)
                return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("User is no found").Build());

            return Ok(new ApiResultBuilder().SetSuccess(true).SetData(user).Build());
        }
    }
}
