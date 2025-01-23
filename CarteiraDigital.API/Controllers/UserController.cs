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
using System.Net;

namespace CarteiraDigital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Token>> Authenticate([FromBody] Credentials credentials)
        {
            if (credentials is null)
                return BadRequest(new { message = "credentials is required" });

            if (string.IsNullOrWhiteSpace(credentials.Username))
                return BadRequest(new { message = "Username is required" });

            if (string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest(new { message = "Password is required" });

            var user = await _userRepository.GetByUsernameAsync(credentials.Username);

            if (user is null)
                return NotFound(new { message = "User no found" });

            if (!PasswordSecurity.VerifyPassword(credentials.Password, user.Password))
                return BadRequest(new { message = "Invalid credentials" });

            string accessToken = _tokenService.GenerateToken(credentials);

            return Ok(new Token
            {
                UserId = user.Id,
                AccessToken = accessToken,
            });
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResult>> Create([FromBody] CreateUserDTO request)
        {
            if (request is null)
                return BadRequest(new ApiResultBuilder().SetMessage("credentials is required").SetSuccess(false).Build());

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

            await _userRepository.SaveAsync(createdUser);

            createdUser.Password = "";

            return Ok(new ApiResultBuilder().SetSuccess(true).SetData(createdUser).Build());
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResult>> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new ApiResultBuilder().SetSuccess(false).SetMessage("id is required").Build());

            Domain.Entities.User? user = await _userRepository.GetByIdAsync(id);

            if (user is null)
                return NotFound(new ApiResultBuilder().SetSuccess(false).SetMessage("User is no found").Build());

            return Ok(new ApiResultBuilder().SetSuccess(true).SetData(user).Build());
        }
    }
}
