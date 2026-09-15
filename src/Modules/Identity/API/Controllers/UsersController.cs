using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskHub.Modules.Identity.Application.Queries.GetCurrentUser;

namespace TaskHub.Modules.Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ISender _sender;
        public UsersController(ISender sender)
        {
            _sender = sender;
        }
        [Authorize]

        [HttpGet("me")]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var user = await _sender.Send(
                new GetCurrentUserQuery(),
                cancellationToken);

            return Ok(user);
        }

    }
}
