using MeetingsService.Core.Entities;
using MinimalMeet.Common.Interfaces;
using MeetingsService.Core.Queries;

namespace MeetingsService.Core.Interfaces;

public interface IAttendeeInfosRepository : IRepository<AttendeeInfo, int>
{
    Task<AttendeeInfo?> GetByUserIdAndMeetingIdAsync(int meetingId, Guid userId);
    Task<List<AttendeeInfo>> GetRoleRequestsForHostAsync(Guid hostId, Query query);
    Task<bool> HasConflictsAsync(int meetingId, Guid userId, DateTime startTime, DateTime endTime);
}
