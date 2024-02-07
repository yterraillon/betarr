using Infrastructure.Betaseries;
using Microsoft.AspNetCore.Mvc;

namespace betarr;

[Route("/betaseries/authentication/token")]
[ApiController]
public class CallbackController(BetaseriesApiClient client) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string code)
    {
        if (string.IsNullOrEmpty(code)) return BadRequest();

        var token = await client.GetAccessToken(code);

        return Ok(token);
    }
}