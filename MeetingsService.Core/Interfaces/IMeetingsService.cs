using MeetingsService.Core.Entities;
using MeetingsService.Core.Queries;
using MinimalMeet.Common.Enums;

namespace MeetingsService.Core.Interfaces;

public interface IMeetingsService
{
    Task<Meeting> CreateMeetingAsync(Meeting meeting, List<Topic> topics, Location location, Dictionary<Guid, AttendeeRoles> attendeeInfos);
    Task<Meeting?> GetMeetingByIdAsync(int id);
    Task<List<Meeting>> GetAllMeetingsAsync();
    Task<bool> DeleteMeetingAsync(int id);
    Task<bool> UpdateMeetingAsync(int meetingId, Meeting meeting, List<Topic> topics, Location location, Dictionary<Guid, AttendeeRoles> attendeeInfos);
    Task<Meeting> GetMeetingWithDetailsAsync(int meetingId);
    Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId, Query query);
    Task<List<PendingMeeting>> GetPendingMeetingsAsync(Guid userId, Query query);
    Task<List<Meeting>> GetPassedMeetingsAsync(Guid userId, Query query);
    Task<bool> CancelMeetingAsync(int meetingId, Guid userId);
    Task<List<Meeting>> GetHostedMeetingsAsync(Guid hostId, Query query);
    Task<List<Meeting>> GetAttendingMeetingsAsync(Guid userId, Query query);
}
