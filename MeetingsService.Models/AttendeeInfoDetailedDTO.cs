using MinimalMeet.Common.Enums;

namespace MeetingsService.Models;

public class AttendeeInfoDetailedDTO : AttendeeInfoDTO
{
    public AttendeeInfoStatuses Status { get; set; }
    public AttendeeRoles? RequestedRole { get; set; }
}
