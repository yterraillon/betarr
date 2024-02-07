using Infrastructure.Sonarr;
using Microsoft.AspNetCore.Mvc;

namespace betarr;

[Route("/sonarr/shows")]
[ApiController]
public class SonarrController(SonarrApiClient sonarrApiClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMoviesFromRadarr()
    {
        var shows = await sonarrApiClient.GetTvShows();

        return Ok(shows);
    }
}