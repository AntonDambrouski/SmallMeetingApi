using MeetingsService.Core.Entities;
using MeetingsService.Core.Extensions;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using MeetingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MinimalMeet.Common.Enums;
using MinimalMeet.Common.Repositories;

namespace MeetingsService.Infrastructure.Repositories;

public class MeetingsRepository(MeetingsContext context)
    : RepositoryBase<Meeting, int, MeetingsContext>(context), IMeetingsRepository
{
    public async Task<Meeting?> GetMeetingWithDetailsAsync(int meetingId)
    {
        var list = new List<int>(); // Fill 10_000_000

        var listEnum = list.Where(i => i == 1); // 2s

        var list1 = listEnum.ToList();
        var list2 = listEnum.ToList();


        return await _context.Meetings
            .Where(m => m.Id == meetingId)
            //.Include(m => m.Location)
            //.Include(m => m.Topics)
            //.Include(m => m.AttendeeInfos)
            .FirstOrDefaultAsync();
    }

    public async Task<List<PendingMeeting>> GetPendingMeetingsAsync(Guid userId, Query query)
    {
        return await _context.Meetings
            .Where(m => m.Status == MeetingStatuses.Upcoming
                        && m.StartTime >= DateTime.UtcNow /*this is a temp fix for demo, ideally we want to filter by status*/
                && m.AttendeeInfos.Any(ai => ai.UserId == userId && ai.Status == AttendeeInfoStatuses.Invited))
            .Include(m => m.Location)
            .SearchByTerm(query.SearchTerm)
            .OrderByDescending(m => m.StartTime)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(m => new PendingMeeting
            {
                Id = m.Id,
                Title = m.Title,
                StartTime = m.StartTime,
                Duration = m.Duration,
                Status = m.Status,
                HostId = m.HostId,
                InvitationMessage = m.InvitationMessage,
                Location = m.Location,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Meeting>> GetPassedMeetingsAsync(Guid userId, Query query)
    {
        return await _context.Meetings
             .Where(m => (m.StartTime < DateTime.UtcNow || /*this is a temp fix for demo, ideally we want to filter by status*/m.Status == MeetingStatuses.Finished) && (m.HostId == userId
                || m.AttendeeInfos.Any(ai => ai.UserId == userId && ai.Status == AttendeeInfoStatuses.Accepted)))
            .Include(m => m.Location)
            .Include(m => m.Topics)
            .SearchByTerm(query.SearchTerm)
            .OrderByDescending(m => m.StartTime)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(m => new Meeting
            {
                Id = m.Id,
                Title = m.Title,
                StartTime = m.StartTime,
                Status = m.Status,
                HostId = m.HostId,
                Location = m.Location,
                Topics = m.Topics,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Meeting>> GetUpcomingMeetingsAsync(Guid userId, Query query)
    {
        return await _context.Meetings
            .Where(m => m.Status == MeetingStatuses.Upcoming && (m.HostId == userId
                || m.AttendeeInfos.Any(ai => ai.UserId == userId && ai.Status == AttendeeInfoStatuses.Accepted))
                && m.StartTime >= DateTime.UtcNow /*this is a temp fix for demo, ideally we want to filter by status*/)
            .Include(m => m.Location)
            .Include(m => m.Topics)
            .SearchByTerm(query.SearchTerm)
            .OrderByDescending(m => m.StartTime)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(m => new Meeting
            {
                Id = m.Id,
                Title = m.Title,
                StartTime = m.StartTime,
                Status = m.Status,
                HostId = m.HostId,
                Duration = m.Duration,
                Location = m.Location,
                Topics = m.Topics,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Meeting>> GetHostedMeetingsAsync(Guid hostId, Query query)
    {
        return await _context.Meetings
            .Where(m => m.HostId == hostId && m.Status == MeetingStatuses.Upcoming)
            .Include(m => m.Location)
            .OrderByDescending(m => m.StartTime)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(m => new Meeting
            {
                Id = m.Id,
                Title = m.Title,
                StartTime = m.StartTime,
                Status = m.Status,
                HostId = m.HostId,
                Duration = m.Duration,
                Location = m.Location,
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Meeting>> GetAttendingMeetingsAsync(Guid userId, Query query)
    {
        return await _context.Meetings
            .Where(m => m.Status == MeetingStatuses.Upcoming && m.HostId != userId
                && m.AttendeeInfos.Any(ai => ai.UserId == userId && ai.Status == AttendeeInfoStatuses.Accepted))
            .Include(m => m.Location)
            .OrderByDescending(m => m.StartTime)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(m => new Meeting
            {
                Id = m.Id,
                Title = m.Title,
                StartTime = m.StartTime,
                Status = m.Status,
                HostId = m.HostId,
                Duration = m.Duration,
                Location = m.Location,
            })
            .AsNoTracking()
            .ToListAsync();
    }
}
