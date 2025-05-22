namespace MeetingsService.Models;

public class PendingMeetingDTO : MeetingDTO
{
    public string InvitationMessage { get; set; }
    public bool HasConflicts { get; set; }
    public Guid HostId { get; set; }
}
