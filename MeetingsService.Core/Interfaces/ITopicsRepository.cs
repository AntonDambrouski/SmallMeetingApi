using MeetingsService.Core.Entities;
using MinimalMeet.Common.Interfaces;
using MeetingsService.Core.Queries;

namespace MeetingsService.Core.Interfaces;

public interface ITopicsRepository : IRepository<Topic, int>
{
    Task<List<Topic>> SearchByTermAsync(Query query);
    Task<Dictionary<string, Topic>> GetExistingByNamesAsync(HashSet<string> topicsNames);
}
