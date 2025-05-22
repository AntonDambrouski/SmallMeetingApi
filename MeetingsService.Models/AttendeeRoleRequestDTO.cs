namespace MeetingsService.Models;

public class AttendeeRoleRequestDTO
{
    public int MeetingId { get; set; }
    public Guid UserId { get; set; }
    public string MeetingTitle { get; set; }
    public string RequestedRole { get; set; }
}
