using Casbin;
using Microsoft.AspNetCore.Mvc;

namespace HRAuthorization.Api.Controllers;

[ApiController]
[Route("api/leave")]
public class LeaveController : ControllerBase
{
    private readonly IEnforcer _enforcer;

    public LeaveController(IEnforcer enforcer) => _enforcer = enforcer;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id, [FromQuery] string dom)
    {
        var user = User.Identity?.Name ?? string.Empty;

        if (await _enforcer.EnforceAsync(user, dom, "api.leave", "read"))
        {
            return Ok();
        }

        return Forbid();
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(string id, [FromQuery] string dom)
    {
        var user = User.Identity?.Name ?? string.Empty;

        if (await _enforcer.EnforceAsync(user, dom, "api.leave", "approve"))
        {
            return Ok();
        }

        return Forbid();
    }
}

