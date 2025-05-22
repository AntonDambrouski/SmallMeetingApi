using MeetingsService.Core.Entities;
using MeetingsService.Core.Extensions;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Queries;
using MeetingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MinimalMeet.Common.Repositories;

namespace MeetingsService.Infrastructure.Repositories;

public class LocationsRepository(MeetingsContext context)
    : RepositoryBase<Location, int, MeetingsContext>(context), ILocationsRepository
{
    public async Task<Location?> GetByPlaceAsync(string place)
    {
        return await _context.Locations.FirstOrDefaultAsync(l => l.Place.ToLower() == place.ToLower());
    }

    public async Task<List<Location>> SearchByTermAndTypeAsync(LocationQuery query)
    {
        return await _context.Locations
            .SearchByTermAndType(query.SearchTerm, query.IsOnline)
            .Skip(query.Skip)
            .Take(query.Take)
            .OrderByDescending(l => l.Place)
            .AsNoTracking()
            .ToListAsync();
    }
}
