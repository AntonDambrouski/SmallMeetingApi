namespace MeetingsService.Core.Entities;

public class PendingMeeting : Meeting
{
    public bool HasConflicts { get; set; }
}
