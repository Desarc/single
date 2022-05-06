using System.Text.Json;

namespace singleApp.Data;

public class MusixmatchClient
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public MusixmatchClient(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    private readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public string ApiKey => _configuration.GetSection("MusixmatchApiKey").Value;

    public async Task<HttpResponseMessage> GetAsync(string apiUrl)
    {
        var httpClient = _httpClientFactory.CreateClient("Musixmatch");

        apiUrl = $"{apiUrl}&apikey={ApiKey}";

        var responseMessage = await httpClient.GetAsync(apiUrl);

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to get data from {httpClient.BaseAddress}/{apiUrl}");
        }

        return responseMessage;
    }

    public async Task<string> GetJsonAsync(string apiUrl)
    {
        var responseMessage = await GetAsync(apiUrl);

        return await responseMessage.Content.ReadAsStringAsync(); ;
    }

    public async Task<MusixmatchChartTrack[]> GetChartAsync(string country)
    {
        var apiUrl = string.Format(Constants.ChartsUrl, country);

        var contentJson = await GetJsonAsync(apiUrl);

        var response = JsonSerializer.Deserialize<MusixmatchChartResponse>(contentJson, SerializerOptions);

        return response.Message.Body.Track_List;
    }

    public async Task<MusixmatchChartTrackData> GetSongFromChartAsync()
    {
        var chart = await GetChartAsync(Constants.CountryNO);

        var random = new Random();
        var randomTrackNo = random.Next(0, chart.Length - 1);

        return chart[randomTrackNo].Track;
    }

    public async Task<MusixmatchLyricsData> GetLyricsForTrackAsync(int trackId)
    {
        var apiUrl = string.Format(Constants.LyricsUrl, trackId);

        var contentJson = await GetJsonAsync(apiUrl);

        var response = JsonSerializer.Deserialize<MusixmatchLyricsResponse>(contentJson, SerializerOptions);

        return response.Message.Body.Lyrics;
    }
}