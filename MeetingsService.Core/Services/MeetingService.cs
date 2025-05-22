using MeetingsService.Core.Entities;
using MeetingsService.Core.Exceptions;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using Microsoft.Extensions.Logging;
using MinimalMeet.Common.Enums;

namespace MeetingsService.Core.Services;

public class MeetingService : IMeetingsService
{
    private readonly ILogger<MeetingService> _logger;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly ITopicsRepository _topicsRepository;
    private readonly ILocationsRepository _locationsRepository;
    private readonly IAttendeeInfosRepository _attendeeInfosRepository;

    public MeetingService(ILogger<MeetingService> logger,
        IMeetingsRepository meetingsRepository,
        ITopicsRepository topicsRepository,
        ILocationsRepository locationsRepository,
        IAttendeeInfosRepository attendeeInfosRepository)
    {
        _logger = logger;
        _meetingsRepository = meetingsRepository;
        _topicsRepository = topicsRepository;
        _locationsRepository = locationsRepository;
        _attendeeInfosRepository = attendeeInfosRepository;
    }

    public async Task<bool> CancelMeetingAsync(int meetingId, Guid userId)
    {
        try
        {
            var meeting = await _meetingsRepository.GetByIdAsync(meetingId)
                ?? throw new ArgumentException($"Could not find meeting with id: {meetingId}");

            if (meeting.HostId != userId) throw new ApplicationException("You are not allowed to cancel this meeting.");

            meeting.Status = MeetingStatuses.Canceled;

            return await _meetingsRepository.UpdateAsync(meeting);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when canceling meeting with id: {id}", meetingId);
            throw new DomainException("Could not cancel meeting.");
        }
    }

    public async Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId, Query query)
    {
        try
        {
            return await _meetingsRepository.GetUpcomingMeetingsAsync(userId, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting user's upcoming meetings");
            throw new DomainException("Could not get upcoming meetings.");
        }
    }

    public async Task<List<PendingMeeting>> GetPendingMeetingsAsync(Guid userId, Query query)
    {
        try
        {
            var meetings = await _meetingsRepository.GetPendingMeetingsAsync(userId, query);
            foreach (var meeting in meetings)
            {
                var endTime = meeting.StartTime.Add(meeting.Duration);
                meeting.HasConflicts = await _attendeeInfosRepository.HasConflictsAsync(meeting.Id, userId, meeting.StartTime, endTime);
            }

            return meetings;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting user's pending meetings");
            throw new DomainException("Could not get pending meetings.");
        }
    }

    public async Task<List<Meeting>> GetPassedMeetingsAsync(Guid userId, Query query)
    {
        try
        {
            return await _meetingsRepository.GetPassedMeetingsAsync(userId, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting user's passed meetings");
            throw new DomainException("Could not get passed meetings.");
        }
    }

    public async Task<List<Meeting>> GetHostedMeetingsAsync(Guid hostId, Query query)
    {
        try
        {
            return await _meetingsRepository.GetHostedMeetingsAsync(hostId, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting user's hosted meetings");
            throw new DomainException("Could not get hosted meetings.");
        }
    }

    public async Task<List<Meeting>> GetAttendingMeetingsAsync(Guid userId, Query query)
    {
        try
        {
            return await _meetingsRepository.GetAttendingMeetingsAsync(userId, query);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting user's upcoming meetings");
            throw new DomainException("Could not get upcoming meetings.");
        }
    }

    public async Task<Meeting> CreateMeetingAsync(Meeting meeting, List<Topic> topics, Location location, Dictionary<Guid, AttendeeRoles> attendeeInfos)
    {
        try
        {
            var topicsNames = topics.Select(t => t.Name).ToHashSet();
            meeting.Topics = await GetExistingAndNewTopicsAsync(topicsNames);

            var existingLocation = await _locationsRepository.GetByPlaceAsync(location.Place);
            meeting.Location = existingLocation ?? new Location { Online = location.Online, Place = location.Place };

            meeting.AttendeeInfos = attendeeInfos.Select(ai => new AttendeeInfo
            {
                UserId = ai.Key,
                Role = ai.Value,
                Status = AttendeeInfoStatuses.Invited,
            }).ToList();

            meeting.Created = meeting.Modified = DateTime.UtcNow;
            meeting.Status = MeetingStatuses.Upcoming;
            return await _meetingsRepository.CreateAsync(meeting);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when creating meeting");
            throw new DomainException("Could not create meeting");
        }
    }

    public async Task<bool> DeleteMeetingAsync(int id)
    {
        try
        {
            return await _meetingsRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when deleting meeting with id: {id}", id);
            throw new DomainException("Could not delete meeting.");
        }
    }

    public async Task<List<Meeting>> GetAllMeetingsAsync()
    {
        return await _meetingsRepository.GetAllAsync();
    }

    public async Task<Meeting?> GetMeetingByIdAsync(int id)
    {
        return await _meetingsRepository.GetByIdAsync(id);
    }

    public async Task<Meeting> GetMeetingWithDetailsAsync(int meetingId)
    {
        try
        {
            var meeting = await _meetingsRepository.GetMeetingWithDetailsAsync(meetingId);
            if (meeting == null)
            {
                _logger.LogInformation("Could not find meeting with id: {meetingId}", meetingId);
                throw new ArgumentException($"Could not find meeting's details.");
            }

            return meeting;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when getting detailed meeting info with id: {meetingId}", meetingId);
            throw new DomainException("Could not get meeting's details.");
        }
    }

    public async Task<bool> UpdateMeetingAsync(int meetingId, Meeting meeting, List<Topic> topics, Location location, Dictionary<Guid, AttendeeRoles> attendeeInfos)
    {
        try
        {
            var savedMeeting = await _meetingsRepository.GetByIdAsync(meetingId)
                ?? throw new ArgumentException($"Could not find meeting with id: {meetingId}");

            if (meeting.ETag != savedMeeting.ETag)
            {
                _logger.LogInformation("Meeting with id: {meetingId} was updated by another user", meetingId);
                throw new ArgumentException("The meeting was updated by another user. Please, refresh the page and try again.");
            }

            if (!savedMeeting.Location.Place.Equals(location.Place, StringComparison.OrdinalIgnoreCase))
            {
                var savedLocation = await _locationsRepository.GetByPlaceAsync(location.Place);
                savedMeeting.Location = savedLocation ?? new Location { Place = location.Place, Online = location.Online };
            }

            var updatedTopics = await GetUpdatedTopicsAsync(meeting.Topics, topics);
            if (updatedTopics.Count != 0)
            {
                meeting.Topics = updatedTopics;
            }

            var savedAttendeeInfos = savedMeeting.AttendeeInfos.ToDictionary(ai => ai.UserId, ai => ai);
            var attendeeInfoIds = attendeeInfos.Keys.ToHashSet();
            foreach (var attendeeInfo in attendeeInfos)
            {
                if (savedAttendeeInfos.TryGetValue(attendeeInfo.Key, out var savedAttendeeInfo))
                {
                    savedAttendeeInfo.Role = attendeeInfo.Value;
                    meeting.AttendeeInfos.Add(savedAttendeeInfo);
                }
                else
                {
                    meeting.AttendeeInfos.Add(new AttendeeInfo
                    {
                        UserId = attendeeInfo.Key,
                        Role = attendeeInfo.Value,
                        Status = AttendeeInfoStatuses.Invited,
                    });
                }
            }

            var attendeesToRemove = savedMeeting.AttendeeInfos
                .Where(ai => !attendeeInfoIds.Contains(ai.UserId))
                .ToList();

            foreach (var attendeeToRemove in attendeesToRemove)
            {
                meeting.AttendeeInfos.Remove(attendeeToRemove);
            }

            meeting.ETag++;
            meeting.Modified = DateTime.UtcNow;
            return await _meetingsRepository.UpdateAsync(meeting);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred when updating meeting with id: {id}", meetingId);
            throw new DomainException("Could not update meeting.");
        }
    }

    private async Task<List<Topic>> GetUpdatedTopicsAsync(ICollection<Topic> savedTopics, List<Topic> updatedTopics)
    {
        var savedTopicsNames = savedTopics.Select(t => t.Name).ToHashSet();
        var updatedTopicsNames = updatedTopics.Select(t => t.Name).ToHashSet();

        if (savedTopicsNames.Count == updatedTopicsNames.Count
            || savedTopicsNames.All(updatedTopicsNames.Contains))
        {
            return [];
        }

        return await GetExistingAndNewTopicsAsync(updatedTopicsNames);
    }

    private async Task<List<Topic>> GetExistingAndNewTopicsAsync(HashSet<string> topicsNames)
    {
        var savedTopics = await _topicsRepository.GetExistingByNamesAsync(topicsNames);

        return topicsNames
            .Select(n => savedTopics.TryGetValue(n, out var topic) ? topic : new Topic { Name = n })
            .ToList();
    }
}
