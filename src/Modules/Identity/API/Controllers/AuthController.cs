using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Modules.Identity.Application.Commands.Login;
using TaskHub.Modules.Identity.Application.Commands.Register;

namespace TaskHub.Modules.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
        {

            var user = await _sender.Send(request, cancellationToken);
            return Created($"/api/Identity/{user}",
        user);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _sender.Send(request, cancellationToken);

            return Ok(user);
        }

    }
}
