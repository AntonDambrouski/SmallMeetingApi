using MeetingsService.Core.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MinimalMeet.Common.Entities;

namespace MeetingsService.Core.Entities;

[Table("Locations", Schema = "dbo")]
public class Location : EntityBase<int>
{
    [MaxLength(AppConstants.LocationPlaceMaxLength)]
    public string Place { get; set; }
    public bool Online { get; set; }
}
