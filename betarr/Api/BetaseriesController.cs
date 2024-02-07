using Infrastructure.Betaseries;
using Infrastructure.Betaseries.Models;
using Infrastructure.Radarr;
using Microsoft.AspNetCore.Mvc;

namespace betarr
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetaseriesController(BetaseriesApiClient betaseriesApiClient, RadarrApiClient radarrApiClient): ControllerBase
    {
        [HttpGet]
        [Route("betaseries/movies")]
        public async Task<IActionResult> GetMovies([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();
            var watchedMovies = await betaseriesApiClient.GetWatchedMovies(code);
            return Ok(watchedMovies);
        }
        
        [HttpGet]
        [Route("betaseries/shows")]
        public async Task<IActionResult> GetShows([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();
            
            var shows = await betaseriesApiClient.GetShows(code);

            return Ok(shows);
        }
        
        [HttpGet]
        [Route("betaseries/movies-diff")]
        public async Task<IActionResult> DisplayBetaseriesMoviesNotInRadarr([FromQuery] string code)
        {
            if (string.IsNullOrEmpty(code)) return BadRequest();

            var betaseriesMovies = new List<BsMovie>();
            var watchedMovies = (await betaseriesApiClient.GetWatchedMovies(code)).movies.ToList();
            var unWatchedMovies = (await betaseriesApiClient.GetUnwatchedMovies(code)).movies.ToList();

            betaseriesMovies.AddRange(watchedMovies);
            betaseriesMovies.AddRange(unWatchedMovies);

            var movies = betaseriesMovies.Select(betaseriesMovie => 
                new Movie(Guid.NewGuid(), betaseriesMovie.imdb_id, betaseriesMovie.title, betaseriesMovie.tmdb_id, true, false )).ToList();

            var radarrMovies = await radarrApiClient.GetMovies();

            foreach (var movie in movies)
            {
                if (radarrMovies.Select(r => r.imdbId).Contains(movie.imdbId))
                {
                    movie.inRadarr = true;
                }
            }

            foreach (var radarrMovie in radarrMovies)
            {
                if (!movies.Select(m => m.imdbId).Contains(radarrMovie.imdbId))
                {
                    movies.Add(new Movie(Guid.NewGuid(), radarrMovie.imdbId, radarrMovie.title, radarrMovie.tmdbId.ToString(), false, true));
                }
            }
            
            return Ok(movies);
        }
    }
}

public class Movie(Guid id, string imdbId, string title, string tmdbId, bool inBs, bool inRadarr)
{
    public Guid id { get; init; } = id;
    public string imdbId { get; set; } = imdbId;
    public string title { get; set; } = title;
    public string tmdbId { get; set; } = tmdbId;
    public bool inBs { get; set; } = inBs;
    public bool inRadarr { get; set; } = inRadarr;
}
