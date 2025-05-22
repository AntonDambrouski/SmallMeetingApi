using MinimalMeet.Common.Entities;
using MinimalMeet.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsService.Core.Entities;

[Table("AttendeeInfos", Schema = "dbo")]
public class AttendeeInfo : EntityBase<int>
{
    public Guid UserId { get; set; }
    public int MeetingId { get; set; }
    public AttendeeRoles Role { get; set; }
    public AttendeeRoles? RequestedRole { get; set; }
    public AttendeeInfoStatuses Status { get; set; }

    public virtual Meeting Meeting { get; set; }
}
