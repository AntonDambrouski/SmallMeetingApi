using MeetingsService.Core.Entities;
using MeetingsService.Core.Queries;
using MinimalMeet.Common.Enums;

namespace MeetingsService.Core.Interfaces;

public interface IAttendeeInfosService
{
    Task<bool> JoinMeetingAsync(int meetingId, Guid userId);
    Task<bool> AcceptInvitationAsync(int meetingId, Guid userId);
    Task<bool> DeclineInvitationAsync(int meetingId, Guid userId);
    Task<bool> RequestRoleAsync(int meetingId, Guid userId, AttendeeRoles requestedRole);
    Task<List<AttendeeInfo>> GetRequestedRolesAsync(Guid hostId, Query query);
    Task<bool> DeclineRoleRequestAsync(int meetingId, Guid userId);
    Task<bool> AcceptRoleRequestAsync(int meetingId, Guid userId);
}
