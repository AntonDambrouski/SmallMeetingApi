using MinimalMeet.Common.Enums;

namespace MeetingsService.Models;

public class AttendeeInfoDTO
{
    public Guid UserId { get; set; }
    public AttendeeRoles Role { get; set; }
}
