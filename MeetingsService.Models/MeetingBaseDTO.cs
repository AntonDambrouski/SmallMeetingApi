namespace MeetingsService.Models;

public class MeetingBaseDTO
{
    public int MeetingId { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
}
