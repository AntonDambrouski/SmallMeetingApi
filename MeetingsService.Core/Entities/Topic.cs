using MinimalMeet.Common.Entities;
using MeetingsService.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeetingsService.Core.Entities;

[Table("Topics", Schema = "dbo")]
public class Topic : EntityBase<int>
{
    [MaxLength(AppConstants.TopicNameMaxLength)]
    public string Name { get; set; }

    public virtual ICollection<Meeting> Meetings { get; set; } = [];
}
