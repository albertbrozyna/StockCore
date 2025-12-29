using Asp.Versioning;
using StockCore.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockCore.Application.Features.Users.Commands.Block;
using StockCore.Application.Features.Users.Commands.UploadProfilePhoto;
using StockCore.Application.Features.Users.Queries.GetAllUsers;
using StockCore.Application.Features.Users.Queries.GetUserByEmail;
using StockCore.Application.Features.Users.Queries.GetUserById;

namespace StockCore.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/users")] 
    [Tags("Users")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;

        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllUsers.Response), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _sender.Send(new GetAllUsers.Query(), ct);
            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _sender.Send(new GetUserById.Query(id), ct);
            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetByEmail(string email, CancellationToken ct)
        {
            var result = await _sender.Send(new GetUserByEmail.Query(email), ct);
            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }

        [HttpPost("{id:guid}/block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Block(Guid id, CancellationToken ct)
        {
            var result = await _sender.Send(new BlockUser.Command(id), ct);
            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }

        [HttpPost("upload")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UploadProfilePhoto(IFormFile file, CancellationToken ct)
        {
            var result = await _sender.Send(new UploadProfilePhoto.Command(file), ct);
            return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
        }
    }
}