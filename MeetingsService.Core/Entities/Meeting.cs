using MeetingsService.Core.Constants;
using MinimalMeet.Common.Entities;
using MinimalMeet.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsService.Core.Entities;

[Table("Meetings", Schema = "dbo")]
public class Meeting : EntityBase<int>
{ 
    [MaxLength(AppConstants.MeetingTitleMaxLength)]
    public string Title { get; set; }

    [MaxLength(AppConstants.MeetingDescriptionMaxLength)]
    public string? Description { get; set; }
    public Guid HostId { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Created { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime StartTime { get; set; }
    public MeetingStatuses Status { get; set; }
    public int LocationId { get; set; }

    [MaxLength(AppConstants.MeetingInvitationMessageMaxLength)]
    public string? InvitationMessage { get; set; }
    public bool NotifyBefore { get; set; }
    public int ETag { get; set; }

    public virtual Location Location { get; set; }
    public virtual ICollection<Feedback> Feedbacks { get; set; } = [];
    public virtual ICollection<Topic> Topics { get; set; } = [];
    public virtual ICollection<AttendeeInfo> AttendeeInfos { get; set; } = [];
}
