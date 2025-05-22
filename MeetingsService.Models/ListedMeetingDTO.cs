
namespace MeetingsService.Models;

public class ListedMeetingDTO : MeetingDTO
{
    public List<string> Topics { get; set; }
    public bool IsUserHosting { get; set; }
    public Guid HostId { get; set; }
}
