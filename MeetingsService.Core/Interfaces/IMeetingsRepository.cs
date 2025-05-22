using MeetingsService.Core.Entities;
using MinimalMeet.Common.Interfaces;
using MeetingsService.Core.Queries;

namespace MeetingsService.Core.Interfaces;

public interface IMeetingsRepository : IRepository<Meeting, int>
{
    Task<Meeting?> GetMeetingWithDetailsAsync(int meetingId);
    Task<List<Meeting>> GetAttendingMeetingsAsync(Guid userId, Query query);
    Task<List<Meeting>> GetHostedMeetingsAsync(Guid hostId, Query query);
    Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId, Query query);
    Task<List<Meeting>> GetPassedMeetingsAsync(Guid userId, Query query);
    Task<List<PendingMeeting>> GetPendingMeetingsAsync(Guid userId, Query query);
}
