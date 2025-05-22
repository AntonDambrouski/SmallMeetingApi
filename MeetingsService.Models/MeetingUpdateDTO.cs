using MinimalMeet.Common.Enums;

namespace MeetingsService.Models;

public class MeetingUpdateDTO : MeetingBaseDTO
{
    public MeetingStatuses Status { get; set; }
    public string Description { get; set; }
    public string InvitationMessage { get; set; }
    public bool NotifyBefore { get; set; }
    public int ETag { get; set; }
    public List<TopicDTO> Topics { get; set; }
    public LocationDTO Location { get; set; }
    public Dictionary<Guid, AttendeeRoles> AttendeeInfos { get; set; }
}
