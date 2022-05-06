namespace singleApp.Data;

public class Constants
{
    public const string MusixmatchApiUrl = "https://api.musixmatch.com/ws/1.1/";

    public const string CountryNO = "no";
    public const string CountryWorld = "XW";
    public const string ChartsUrl = "chart.tracks.get?chart_name=top&page=1&page_size=100&country={0}&f_has_lyrics=1";
    public const string LyricsUrl = "track.lyrics.get?track_id={0}";
}
