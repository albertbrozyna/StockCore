using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockCore.Application.Features.Auth.Commands.Login;
using StockCore.Application.Features.Auth.Commands.Refresh;
using StockCore.Application.Features.Auth.Commands.Register;
using StockCore.Application.Features.Auth.Commands.ResendVerifyCode;
using StockCore.Application.Features.Auth.Commands.VerifyEmail;

namespace StockCore.Api.Controllers
{
    [ApiVersion("1.0")]
    // Dodajemy trasę - v{version:apiVersion} automatycznie wstawi "1.0"
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Tags("Auth")]
    public class AuthController : ControllerBase
    {

        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EndpointName("Login")]
        [ProducesResponseType(typeof(Login.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] Login.Command command,CancellationToken ct)
        {
            var result = await _sender.Send(command,ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [EndpointName("Register")]
        [ProducesResponseType(typeof(Register.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Register([FromBody] Register.Command command,CancellationToken ct)
        {
            var result = await _sender.Send(command,ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }


        [HttpPost("refresh")]
        [AllowAnonymous]
        [EndpointName("Refresh")]
        [ProducesResponseType(typeof(RefreshToken.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> Refresh([FromBody] RefreshToken.Command command,CancellationToken ct)
        {
            var result = await _sender.Send(command,ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost("verify-email")]
        [AllowAnonymous]
        [EndpointName("VerifyEmail")]
        [ProducesResponseType(typeof(VerifyEmail.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmail.Command command,CancellationToken ct)
        {
            var result = await _sender.Send(command,ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }


        [HttpPost("resend-verification")]
        [AllowAnonymous]
        [EndpointName("ResendVerification")]
        [ProducesResponseType(typeof(ResendVerificationCode.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public async Task<IActionResult> ResendVerficationCode([FromBody] ResendVerificationCode.Command command,CancellationToken ct)
        {
            var result = await _sender.Send(command,ct);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }
    }
}
