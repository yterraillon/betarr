using Infrastructure.Betaseries;
using Infrastructure.Radarr;
using Infrastructure.Settings;
using Infrastructure.Sonarr;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        const string BetarrUserAgent = "betarr";
        
        services.Configure<BetaseriesSettings>(configuration.GetSection("Betaseries"));
        
        var betaseriesSettings = configuration.GetSection("Betaseries").Get<BetaseriesSettings>();
        services.AddHttpClient<BetaseriesApiClient>("betaseries-api", client =>
        {
            client.BaseAddress = new Uri("https://api.betaseries.com");
            client.DefaultRequestHeaders.Add("X-BetaSeries-Version", "3.0");
            client.DefaultRequestHeaders.Add("X-BetaSeries-Key", betaseriesSettings.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", BetarrUserAgent);
        });
        
        var radarrSettings = configuration.GetSection("Radarr").Get<RadarrSettings>();
        services.AddHttpClient<RadarrApiClient>("radarr-api", client =>
        {
            client.BaseAddress = new Uri("http://192.168.1.101:7878");
            client.DefaultRequestHeaders.Add("X-Api-Key", radarrSettings.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", BetarrUserAgent);
        });
        
        var sonarrSettings = configuration.GetSection("Sonarr").Get<SonarrSettings>();
        services.AddHttpClient<SonarrApiClient>("sonarr-api", client =>
        {
            client.BaseAddress = new Uri("http://192.168.1.101:8989");
            client.DefaultRequestHeaders.Add("X-Api-Key", sonarrSettings.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", BetarrUserAgent);
        });

        return services;
    }
}