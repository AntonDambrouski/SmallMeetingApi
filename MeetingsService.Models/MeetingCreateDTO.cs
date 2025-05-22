using MinimalMeet.Common.Enums;
using System.Text.Json.Serialization;

namespace MeetingsService.Models;

public class MeetingCreateDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public string Description { get; set; }
    public Guid HostId { get; set; }
    public TimeSpan Duration { get; set; }
    public string InvitationMessage { get; set; }
    public bool NotifyBefore { get; set; }
    public List<TopicDTO> Topics { get; set; }
    public LocationDTO Location { get; set; }

    public Dictionary<Guid, AttendeeRoles> AttendeeInfos { get; set; }
}
