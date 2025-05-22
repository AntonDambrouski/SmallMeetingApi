namespace MeetingsService.Models;

public class MeetingInvitationDTO : MeetingDTO
{
    public string Description { get; set; }
    public LocationDTO Location { get; set; }
    public List<TopicDTO> Topics { get; set; }
    public TimeSpan Duration { get; set; }
    public List<AttendeeInfoDTO> AttendeeInfos { get; set; }
}
