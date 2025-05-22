using MeetingsService.Core.Entities;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using MeetingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MinimalMeet.Common.Enums;
using MinimalMeet.Common.Repositories;

namespace MeetingsService.Infrastructure.Repositories;

public class AttendeeInfosRepository(MeetingsContext context)
    : RepositoryBase<AttendeeInfo, int, MeetingsContext>(context), IAttendeeInfosRepository
{
    public async Task<AttendeeInfo?> GetByUserIdAndMeetingIdAsync(int meetingId, Guid userId)
    {
        return await _context.AttendeeInfos
            .FirstOrDefaultAsync(ai => ai.MeetingId == meetingId && ai.UserId == userId);
    }

    public async Task<List<AttendeeInfo>> GetRoleRequestsForHostAsync(Guid hostId, Query query)
    {
        return await _context.AttendeeInfos
            .Include(ai => ai.Meeting)
            .Where(ai => ai.Meeting.HostId == hostId && ai.Meeting.Status == MeetingStatuses.Upcoming && ai.RequestedRole != null)
            .Select(ai => new AttendeeInfo
            {
                MeetingId = ai.MeetingId,
                UserId = ai.UserId,
                Meeting = new Meeting 
                { 
                    Title = ai.Meeting.Title
                },
                RequestedRole = ai.RequestedRole.Value,
            })
            .Skip(query.Skip)
            .Take(query.Take)
            .OrderBy(ai => ai.MeetingId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> HasConflictsAsync(int meetingId, Guid userId, DateTime startTime, DateTime endTime)
    {
        var usersMeetings = await _context.AttendeeInfos
            .Where(ai => ai.MeetingId != meetingId 
                && ai.UserId == userId 
                && ai.Status == AttendeeInfoStatuses.Invited 
                && ai.Meeting.StartTime < endTime)
            .Select(ai =>
            new
            {
                ai.Meeting.StartTime,
                ai.Meeting.Duration
            })
            .ToListAsync();

        return usersMeetings.Any(x => x.StartTime >= startTime && x.StartTime.Add(x.Duration) <= endTime);
    }
}