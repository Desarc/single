namespace singleApp.Data;

public class MusixmatchChartResponse
{
    public MusixmatchChartMessage Message { get; set; }
}

public class MusixmatchChartMessage
{
    public MusixmatchChartHeader Header { get; set; }
    public MusixmatchChartBody Body { get; set; }
}

public class MusixmatchChartHeader
{
    public int Status_Code { get; set; }
    public float Execute_Time { get; set; }
    public int Available { get; set; }
}

public class MusixmatchChartBody
{
    public MusixmatchChartTrack[] Track_List { get; set; }
}

public class MusixmatchChartTrack
{
    public MusixmatchChartTrackData Track { get; set; }
}

public class MusixmatchChartTrackData
{
    public int Track_Id { get; set; }
    public string Track_Name { get; set; }
    public int Track_Rating { get; set; }
    public int Commontrack_Id { get; set; }
    public int Instrumental { get; set; }
    public int Explicit { get; set; }
    public int Has_Lyrics { get; set; }
    public int Has_Subtitles { get; set; }
    public int Album_Id { get; set; }
    public string Album_Name { get; set;}
    public int Artist_Id { get; set; }
    public string Artist_Name { get; set; }
    public string Track_Share_Url { get; set; }
    public string Track_Edit_Url { get; set; }
}