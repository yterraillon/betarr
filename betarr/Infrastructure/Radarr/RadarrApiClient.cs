using Infrastructure.Extensions;

namespace Infrastructure.Radarr;

/// <summary>
/// https://radarr.video/docs/api/
/// </summary>
public class RadarrApiClient(HttpClient client)
{
    public async Task<IEnumerable<RadarrMovie>> GetMovies()
    {
        var url = $"{client.BaseAddress}api/v3/movie";

        var movies = await client.GetJsonObject<List<RadarrMovie>>(url);

        return movies;
    }
}

public record RadarrMovie(int id, string title, string originalTitle, string imdbId, int tmdbId);