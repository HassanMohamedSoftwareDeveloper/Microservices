using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace GoogleMapInfo;

public sealed class GoogleDistanceApi
{
    private readonly IConfiguration _configuration;
    public GoogleDistanceApi(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<GoogleDistanceData> GetMapDistance(string originCity, string destinationCity)
    {
        var apiKey = _configuration["GoogleDistanceApi:ApiKey"];
        var googleDistanceApiUrl = _configuration["GoogleDistanceApi:ApiUrl"];

        googleDistanceApiUrl += $"units=imperial&origins={originCity}&destinations ={destinationCity}&key ={apiKey}";

        using var client = new HttpClient();
        var request = new
        HttpRequestMessage(HttpMethod.Get, new Uri(googleDistanceApiUrl));
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        await using var data = await response.Content.ReadAsStreamAsync();
        var distanceInfo = await
        JsonSerializer.DeserializeAsync<GoogleDistanceData>(data);
        return distanceInfo;
    }
}