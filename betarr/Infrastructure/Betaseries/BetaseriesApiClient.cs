using System.Net.Http.Headers;
using Infrastructure.Extensions;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Infrastructure.Betaseries;

using Models;
public class BetaseriesApiClient(HttpClient client, IOptions<BetaseriesSettings> settings)
{
    private readonly BetaseriesSettings _settings = settings.Value;

    public async Task<string> GetAccessToken(string code)
    {
        // https://developers.betaseries.com/docs/making-requests
        var oauthParameters = new OauthParameters(_settings.ApiKey,
            _settings.ClientSecret,
            "http://localhost:5069/betaseries/authentication/token",
            code);
        
        var accessToken = await client.PostRequest<OauthParameters, AccessToken>("/oauth/access_token", oauthParameters);
        
        return accessToken.access_token;
    }

    
    // TODO : passer le state en parametre 
    // state : string
    // 0 = to watch, 1 = watched, 2 = do not want to watch (default 0)
    public async Task<Movies> GetUnwatchedMovies(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // https://developers.betaseries.com/reference/get-movies-member
        var url = $"{client.BaseAddress}movies/member?state=0";

        var movies = await client.GetJsonObject<Movies>(url);

        return movies;
    }
    
    public async Task<Movies> GetWatchedMovies(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // https://developers.betaseries.com/reference/get-movies-member
        var url = $"{client.BaseAddress}movies/member?state=1";

        var movies = await client.GetJsonObject<Movies>(url);

        return movies;
    }
    
    public async Task<Shows> GetShows(string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        // https://developers.betaseries.com/reference/get-shows-member
        // 100 séries par défaut, max 200. Prévoir pagination
        var url = $"{client.BaseAddress}shows/member";

        var shows = await client.GetJsonObject<Shows>(url);

        return shows;
    }
    
    // TODO : add a logout - call to /member/destroy that kills the token
}