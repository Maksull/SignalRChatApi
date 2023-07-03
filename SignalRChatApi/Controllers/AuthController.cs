using Core.Contracts.Controllers.Auth;
using Core.Mediator.Commands.Auth;
using Core.Mediator.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRChatApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var result = await _mediator.Send(new LoginCommand(loginRequest));

            if (result is not null)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var result = await _mediator.Send(new RegisterCommand(registerRequest));

            if (!result.Any())
            {
                return Ok();
            }

            return BadRequest(new Failed(result));
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailRequest confirmEmailRequest)
        {
            var errors = await _mediator.Send(new ConfirmEmailCommand(confirmEmailRequest));

            if (!errors.Any())
            {
                return Ok();
            }

            return BadRequest(new Failed(errors));
        }

        [HttpGet("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(resetPasswordRequest));

            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpGet("confirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromQuery] ConfirmResetPasswordRequest confirmResetPasswordRequest)
        {
            var errors = await _mediator.Send(new ConfirmResetPasswordCommand(confirmResetPasswordRequest));

            if (!errors.Any())
            {
                return Ok();
            }

            return BadRequest(new Failed(errors));
        }

        [Authorize]
        [HttpGet("userData")]
        public async Task<IActionResult> GetUserData()
        {
            var data = await _mediator.Send(new GetUserDataQuery(User));

            if (data is not null)
            {
                return Ok(data);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminProtected")]
        public IActionResult AdminProtected()
        {
            return Ok();
        }
    }
}
