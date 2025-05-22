using AutoMapper;
using MeetingsService.Api.Helpers;
using MeetingsService.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsService.Api.Endpoints;

public static class LocationsEndpoints
{
    public static void MapLocationsEndpoints(this WebApplication app)
    {
        var locations = app.MapGroup("api/" + GroupNames.Locations);
        locations.MapGet("/search", GetLocationsByNameAsync)
            .WithName(nameof(GetLocationsByNameAsync))
            .WithOpenApi();
    }

    private static async Task<IResult> GetLocationsByNameAsync([AsParameters] LocationQueryParams query, 
        [FromServices] ILocationsRepository locationsRepository,
        [FromServices] IMapper mapper)
    {
        var locations = await locationsRepository.SearchByTermAndTypeAsync(new LocationQuery
        {
            Skip = query.Skip,
            Take = query.Take,
            SearchTerm = query.SearchTerm,
            IsOnline = query.IsLocationOnline
        });

        var locationsDto = mapper.Map<List<LocationDTO>>(locations);
        return Results.Ok(locationsDto);
    }
}
