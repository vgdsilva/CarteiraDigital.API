using CarteiraDigital.Application.Builders;
using CarteiraDigital.Application.Interfaces;
using CarteiraDigital.Domain.DTOs;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Models;
using CarteiraDigital.Domain.Utils;
using CarteiraDigital.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Token>> Authenticate([FromBody] Credentials credentials)
        {
            if (credentials is null)
                return BadRequest(new ApiResultBuilder().SetMessage("credentials is required").SetSuccess(false).Build());

            if (string.IsNullOrWhiteSpace(credentials.Username))
                return BadRequest(new ApiResultBuilder().SetMessage("Username is required").SetSuccess(false).Build());

            if (string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest(new ApiResultBuilder().SetMessage("Password is required").SetSuccess(false).Build());

            var user = await _userRepository.GetByUsernameAsync(credentials.Username);

            if (user is null)
                return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("User is no found").Build());

            if (!PasswordSecurity.VerifyPassword(credentials.Password, user.Password))
                return BadRequest(new ApiResultBuilder().SetMessage("Invalid credentials").SetSuccess(false).Build());

            string accessToken = _tokenService.GenerateToken(credentials);

            return Ok(new ApiResultBuilder()
                .SetSuccess(true)
                .SetData(new Token
                {
                    UserId = user.Id,
                    AccessToken = accessToken,
                })
                .Build()
            );
        }


        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO request)
        {
            if (request is null)
                return BadRequest(new ApiResultBuilder().SetMessage("request is required").SetSuccess(false).Build());

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new ApiResultBuilder().SetMessage("Name is required").SetSuccess(false).Build());

            if (string.IsNullOrWhiteSpace(request.Username))
                return BadRequest(new ApiResultBuilder().SetMessage("Username is required").SetSuccess(false).Build());

            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new ApiResultBuilder().SetMessage("Password is required").SetSuccess(false).Build());

            Domain.Entities.User createdUser = new User
            {
                Name = request.Name,
                Username = request.Username,
                Password = PasswordSecurity.HashPassword(request.Password),
            };

            await _userRepository.SaveChangesAsync(createdUser);

            return CreatedAtAction(nameof(Register), new { UserId = createdUser.Id}, createdUser);
        }
    }
}
