using Infrastructure.Extensions;

namespace Infrastructure.Sonarr;

/// <summary>
/// https://sonarr.tv/docs/api/
/// </summary>
public class SonarrApiClient(HttpClient client)
{
    public async Task<IEnumerable<TvShow>> GetTvShows()
    {
        var url = $"{client.BaseAddress}api/v3/series";

        var tvShows = await client.GetJsonObject<List<TvShow>>(url);

        return tvShows;
    }
}

public record TvShow(int id, string title, string imdbId, string tmdbId);