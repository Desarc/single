namespace singleApp.Data;

public class MusixmatchLyricsResponse
{
    public MusixmatchLyricsMessage Message { get; set; }
}

public class MusixmatchLyricsMessage
{
    public MusixmatchLyricsHeader Header { get; set; }
    public MusixmatchLyricsBody Body { get; set; }
}

public class MusixmatchLyricsHeader
{
    public int Status_Code { get; set; }
    public float Execute_Time { get; set; }
}

public class MusixmatchLyricsBody
{
    public MusixmatchLyricsData Lyrics { get; set; }
}

public class MusixmatchLyricsData
{
    public int Lyrics_Id { get; set; }
    public int Restricted { get; set; }
    public int Instrumental { get; set; }
    public string Lyrics_Body { get; set; }
    public string Lyrics_Language { get; set; }
    public string Script_Tracking_Url { get; set; }
    public string Pixel_Tracking_Url { get; set; }
    public string Lyrics_Copyright { get; set; }
    public string Backlink_Url { get; set; }
}