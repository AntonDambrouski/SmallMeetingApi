using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MinimalMeet.Common.Entities;

namespace MeetingsService.Core.Entities;

[Table("Feedbacks", Schema = "dbo")]
public class Feedback : EntityBase<int>
{
    public Guid UserId { get; set; }
    public int MeetingId { get; set; }
    public int Rating { get; set; }

    [MaxLength(200)]
    public string? Comment { get; set; }
    public DateTime Created { get; set; }

    public virtual Meeting Meeting { get; set; }
}
