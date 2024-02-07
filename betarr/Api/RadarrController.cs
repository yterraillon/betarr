using Infrastructure.Radarr;
using Microsoft.AspNetCore.Mvc;

namespace betarr;

[Route("/radarr/movies")]
[ApiController]
public class RadarrController(RadarrApiClient radarrApiClient) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMoviesFromRadarr()
    {
        var movies = await radarrApiClient.GetMovies();

        return Ok(movies);
    }
}