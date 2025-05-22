using MeetingsService.Core.Entities;
using MeetingsService.Core.Exceptions;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using Microsoft.Extensions.Logging;
using MinimalMeet.Common.Enums;

namespace MeetingsService.Core.Services;

public class AttendeeInfosService : IAttendeeInfosService
{
    private readonly ILogger<AttendeeInfosService> _logger;
    private readonly IAttendeeInfosRepository _attendeeInfosRepository;
    private readonly IMeetingsRepository _meetingsRepository;

    public AttendeeInfosService(ILogger<AttendeeInfosService> logger, IAttendeeInfosRepository attendeeInfosRepository, IMeetingsRepository meetingsRepository)
    {
        _logger = logger;
        _attendeeInfosRepository = attendeeInfosRepository;
        _meetingsRepository = meetingsRepository;
    }

    public async Task<List<AttendeeInfo>> GetRequestedRolesAsync(Guid hostId, Query query)
    {
        try
        {
            return await _attendeeInfosRepository.GetRoleRequestsForHostAsync(hostId, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting requested roles for host with id: {hostId}", hostId);
            throw new DomainException("Could not get the requested roles");
        }
    }

    public async Task<bool> DeclineRoleRequestAsync(int meetingId, Guid userId)
    {
        return await ChangeRoleRequestAsync(meetingId, userId, false);
    }

    public async Task<bool> AcceptRoleRequestAsync(int meetingId, Guid userId)
    {
       return await ChangeRoleRequestAsync(meetingId, userId, true);
    }

    public async Task<bool> RequestRoleAsync(int meetingId, Guid userId, AttendeeRoles requestedRole)
    {
        try
        {
            var attendeeInfo = await _attendeeInfosRepository.GetByUserIdAndMeetingIdAsync(meetingId, userId);
            if (attendeeInfo == null)
            {
                _logger.LogInformation("Could not find attendee info for user id: {userId}, meetingId: {meetingId}", userId, meetingId);
                return false;
            }

            if (attendeeInfo.Status != AttendeeInfoStatuses.Accepted)
            {
                _logger.LogInformation("User with id {userId} tried to request role for meeting with id {meetingId} that is in status {status}.", userId, meetingId, attendeeInfo.Status);
                return false;
            }

            attendeeInfo.RequestedRole = requestedRole;
            return await _attendeeInfosRepository.UpdateAsync(attendeeInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when requesting role for meeting with id: {meetingId}", meetingId);
            throw new DomainException("Could not request the role for the meeting");
        }
    }

    public async Task<bool> AcceptInvitationAsync(int meetingId, Guid userId)
    {
        return await ChangeInvitationStatusAsync(meetingId, userId, AttendeeInfoStatuses.Accepted, nameof(AcceptInvitationAsync));
    }

    public async Task<bool> DeclineInvitationAsync(int meetingId, Guid userId)
    {
        return await ChangeInvitationStatusAsync(meetingId, userId, AttendeeInfoStatuses.Declined, nameof(DeclineInvitationAsync));
    }

    public async Task<bool> JoinMeetingAsync(int meetingId, Guid userId)
    {
        try
        {
            var meeting = await _meetingsRepository.GetByIdAsync(meetingId);
            if (meeting == null)
            {
                _logger.LogInformation("Could not find meeting with id: {meetingId}", meetingId);
                return false;
            }

            var attendeeInfo = await _attendeeInfosRepository.GetByUserIdAndMeetingIdAsync(meetingId, userId);
            if (attendeeInfo != null)
            {
                _logger.LogInformation("User with id {userId} tried to join meeting with id {meetingId} by url while already be as attendee", userId, meetingId);
                attendeeInfo.Status = AttendeeInfoStatuses.Accepted;
                return await _attendeeInfosRepository.UpdateAsync(attendeeInfo);
            }

            await _attendeeInfosRepository.CreateAsync(new AttendeeInfo
            {
                MeetingId = meetingId,
                UserId = userId,
                Role = AttendeeRoles.Participant,
                Status = AttendeeInfoStatuses.Accepted,
            });

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when joining meeting with id: {meetingId}", meetingId);
            throw new DomainException("Could not join the meeting");
        }
    }

    private async Task<bool> ChangeInvitationStatusAsync(int meetingId, Guid userId, AttendeeInfoStatuses status, string? methodName = null)
    {
        try
        {
            var attendeeInfo = await _attendeeInfosRepository.GetByUserIdAndMeetingIdAsync(meetingId, userId);
            if (attendeeInfo == null)
            {
                _logger.LogInformation("{methodName} failed. Could not find attendee info for user id: {userId}, meetingId: {meetingId}", methodName, userId, meetingId);
                return false;
            }

            if (attendeeInfo.Status != AttendeeInfoStatuses.Invited)
            {
                _logger.LogInformation("{methodName} failed. User with id {userId} tried to change meeting status" +
                    " with id {meetingId} that is in status {status}.", methodName, userId, meetingId, attendeeInfo.Status);

                return false;
            }

            attendeeInfo.Status = status;
            return await _attendeeInfosRepository.UpdateAsync(attendeeInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} failed. User with id {userId} tried to accept change state for meeting with id {meetingId}.", methodName, userId, meetingId);
            throw new DomainException("Could not change the meeting status.");
        }
    }

    private async Task<bool> ChangeRoleRequestAsync(int meetingId, Guid userId, bool accept)
    {
        var changeType = accept ? "accept" : "decline";

        try
        {
            var attendeeInfo = await _attendeeInfosRepository.GetByUserIdAndMeetingIdAsync(meetingId, userId);
            if (attendeeInfo is null)
            {
                _logger.LogWarning("Could not find attendee info by meetingId: {meetingId} and userId: {userId} to chagne request role.", meetingId, userId);
                return false;
            }

            if (attendeeInfo.Status != AttendeeInfoStatuses.Accepted)
            {
                _logger.LogInformation("Tried to {changeType} role for attendee info with id {id} wiht status {status}.", changeType, attendeeInfo.Id, attendeeInfo.Status);
                throw new ArgumentException($"You can only {changeType} a role for a meeting that is in status Accepted.");
            }

            if (attendeeInfo.RequestedRole == null)
            {
                _logger.LogInformation("Tried to {changeType} role for attendee info with id {id} without a requested role.", changeType, attendeeInfo.Id);
                return false;
            }

            if (accept)
            {
                attendeeInfo.Role = attendeeInfo.RequestedRole.Value;
            }

            attendeeInfo.RequestedRole = null;
            return await _attendeeInfosRepository.UpdateAsync(attendeeInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when changing role request for attendee info with meetingId: {meetingId} and userId: {userId}", meetingId, userId);
            throw new DomainException($"Could not {changeType} the role request for the meeting");
        }
    }
}
