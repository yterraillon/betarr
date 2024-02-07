using Infrastructure;
using Infrastructure.Settings;
using Microsoft.AspNetCore.WebUtilities;

namespace betarr;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddControllers();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        app.MapControllers();
        
        
        var betaseriesSettings = builder.Configuration.GetSection("Betaseries").Get<BetaseriesSettings>();
        
        app.MapGet("/betaseries/authorize-url", (HttpContext _) =>
            {
                var redirectUrl = "http://localhost:5069/betaseries/authentication/token";
                var apikey = betaseriesSettings!.ApiKey;
                var betaseriesAuthorizeUrl = "https://www.betaseries.com/authorize";

                var parameters = new Dictionary<string, string>
                {
                    {"client_id", apikey},
                    {"redirect_uri", redirectUrl},
                };

                var url = QueryHelpers.AddQueryString(betaseriesAuthorizeUrl, parameters!);
        
                return url;
            })
            .WithName("GetBetaseriesAuthorizeUrl")
            .WithOpenApi();
        
        app.Run();
    }
}