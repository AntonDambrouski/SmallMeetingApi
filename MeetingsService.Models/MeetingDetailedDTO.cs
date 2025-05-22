using MinimalMeet.Common.Enums;

namespace MeetingsService.Models;

public class MeetingDetailedDTO : MeetingBaseDTO
{
    public Guid HostId { get; set; }
    public MeetingStatuses Status { get; set; }
    public bool IsCurrentUserHost { get; set; }
    public string Description { get; set; }
    public string InvitationMessage { get; set; }
    public LocationDTO Location { get; set; }
    public List<TopicDTO> Topics { get; set; }
    public List<Guid> PresenterIds { get; set; }
    public List<Guid> ParticipantIds { get; set; }

    /// <summary>
    /// Note: if IsCurrentUserHost is true, CurrentUserAttendeeInfo will be null
    /// </summary>
    public AttendeeInfoDetailedDTO CurrentUserAttendeeInfo { get; set; }
}
