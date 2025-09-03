using Casbin;
using Microsoft.AspNetCore.Mvc;

namespace HRAuthorization.Api.Controllers;

[ApiController]
[Route("authz")]
public class AuthzController : ControllerBase
{
    private readonly IEnforcer _enforcer;

    public AuthzController(IEnforcer enforcer) => _enforcer = enforcer;

    [HttpGet("check")]
    public async Task<IActionResult> Check([FromQuery] string dom, [FromQuery] string obj, [FromQuery] string act)
    {
        var user = User.Identity?.Name ?? string.Empty;

        if (await _enforcer.EnforceAsync(user, dom, obj, act))
        {
            return NoContent();
        }

        return Forbid();
    }
}
