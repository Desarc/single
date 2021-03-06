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

        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new Exception("'MusixmatchApiKey' not configured");
        }
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
            throw new Exception($"Failed to get data from {httpClient.BaseAddress}/{apiUrl}: {responseMessage.ReasonPhrase}");
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

        Console.WriteLine(contentJson);

        var response = JsonSerializer.Deserialize<MusixmatchChartResponse>(contentJson, SerializerOptions);

        if (response.Message.Header.Status_Code != 200)
        {
            throw new Exception($"Failed to get chart for country '{country}': {response.Message.Header.Status_Code}");
        }

        var trackList = response.Message.Body.Track_List;

        if (trackList.Length == 0)
        {
            throw new Exception($"Chart for country '{country}' contains no tracks");
        }

        return trackList;
    }

    public async Task<MusixmatchChartTrackData> GetRandomSongFromChartAsync()
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

        Console.WriteLine(contentJson);

        var response = JsonSerializer.Deserialize<MusixmatchLyricsResponse>(contentJson, SerializerOptions);

        if (response.Message.Header.Status_Code != 200)
        {
            throw new Exception($"Failed to get lyrics for track '{trackId}': {response.Message.Header.Status_Code}");
        }

        return response.Message.Body.Lyrics;
    }

    public async Task<MusixmatchLyricsData> GetLyricsForCommonTrackAsync(int commonTrackId)
    {
        var apiUrl = string.Format(Constants.LyricsUrlCommonTrack, commonTrackId);

        var contentJson = await GetJsonAsync(apiUrl);

        Console.WriteLine(contentJson);

        var response = JsonSerializer.Deserialize<MusixmatchLyricsResponse>(contentJson, SerializerOptions);

        if (response.Message.Header.Status_Code != 200)
        {
            throw new Exception($"Failed to get lyrics for common track '{commonTrackId}': {response.Message.Header.Status_Code}");
        }

        return response.Message.Body.Lyrics;
    }

    public async Task<MusixmatchLyricsData> GetRandomLyricsAsync()
    {
        var chart = await GetChartAsync(Constants.CountryNO);

        var random = new Random();
        while (true)
        {
            var randomTrackNo = random.Next(0, chart.Length - 1);
            var track = chart[randomTrackNo].Track;

            if (track.Commontrack_Id != 0)
            {
                var lyrics = await GetLyricsForCommonTrackAsync(track.Commontrack_Id);
                if (!lyrics.IsRestricted)
                {
                    return lyrics;
                }
            }
            else
            {
                var lyrics = await GetLyricsForTrackAsync(track.Track_Id);
                if (!lyrics.IsRestricted)
                {
                    return lyrics;
                }
            }
        }
    }
}