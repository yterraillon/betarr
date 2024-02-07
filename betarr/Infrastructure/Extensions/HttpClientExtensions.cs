using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResult?> PostRequest<TArgument, TResult>(this HttpClient client, string relativeUri,
        TArgument request)
    {
        try
        {
            var jsonCommand = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonCommand, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(relativeUri, content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResult>(result);
        }

        catch (Exception ex)
        {
            var uri = new Uri(client.BaseAddress, relativeUri);
        }

        return default;
    }
    
    public static async Task<TResult?> GetJsonObject<TResult>(this HttpClient client,string relativeUri)
    {
        try
        {
            var response = await client.GetAsync(relativeUri);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(result);
        }
        catch (Exception ex)
        {
            var uri = new Uri(client.BaseAddress, relativeUri);
        }

        return default;
    }
}