using MeetingsService.Core.Entities;
using MeetingsService.Core.Extensions;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using MeetingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MinimalMeet.Common.Repositories;

namespace MeetingsService.Infrastructure.Repositories;

public class TopicsRepository(MeetingsContext context)
    : RepositoryBase<Topic, int, MeetingsContext>(context), ITopicsRepository
{
    public async Task<Dictionary<string, Topic>> GetExistingByNamesAsync(HashSet<string> topicsNames)
    {
        return await _context.Topics.Where(t => topicsNames.Contains(t.Name)).ToDictionaryAsync(t => t.Name, t => t);
    }

    public async Task<List<Topic>> SearchByTermAsync(Query query)
    {
        return await _context.Topics
            .SearchByTerm(query.SearchTerm)
            .Skip(query.Skip)
            .Take(query.Take)
            .OrderByDescending(t => t.Name)
            .AsNoTracking()
            .ToListAsync();
    }
}