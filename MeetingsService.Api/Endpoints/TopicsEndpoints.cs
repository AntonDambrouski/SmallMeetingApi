using AutoMapper;
using MeetingsService.Api.Helpers;
using MeetingsService.Core.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MeetingsService.Api.Endpoints;

public static class TopicsEndpoints
{
    public static void MapTopicsEndpoints(this WebApplication app)
    {
        var topics = app.MapGroup("api/" + GroupNames.Topics);
        topics.MapGet("/search", GetTopicsByNameAsync)
            .WithName(nameof(GetTopicsByNameAsync))
            .WithOpenApi();
    }

    private static async Task<IResult> GetTopicsByNameAsync([AsParameters] QueryParams query, 
        [FromServices] ITopicsRepository topicsRepository,
        [FromServices] IMapper mapper)
    {
        var topics = await topicsRepository.SearchByTermAsync(new Query
        {
            Skip = query.Skip,
            Take = query.Take,
            SearchTerm = query.SearchTerm
        });

        var topicsDto = mapper.Map<List<TopicDTO>>(topics);

        return Results.Ok(topicsDto);
    }
}
