using MeetingsService.Core.Entities;
using MinimalMeet.Common.Interfaces;
using MeetingsService.Core.Queries;

namespace MeetingsService.Core.Interfaces;

public interface ILocationsRepository : IRepository<Location, int>
{
    Task<Location?> GetByPlaceAsync(string place);
    Task<List<Location>> SearchByTermAndTypeAsync(LocationQuery query);
}
