using AutoMapper;
using MeetingsService.Api.Helpers;
using MeetingsService.Core.Constants;
using MeetingsService.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using CommonConstants = MinimalMeet.Common.Constants;

namespace MeetingsService.Api.Endpoints;

public static class MeetingsEndpoints
{
    public static void MapMeetingsEndpoints(this WebApplication app)
    {
        var meetings = app.MapGroup("api/" + GroupNames.Meetings);
        meetings.MapGet("", GetAllMeetingsAsync)
            .WithName(nameof(GetAllMeetingsAsync))
            .WithOpenApi();

        meetings.MapGet("/{id:int}", GetMeetingByIdAsync)
            .WithName(nameof(GetMeetingByIdAsync))
            .WithOpenApi();

        meetings.MapGet("/{id:int}/details", GetMeetingWithDetailsAsync)
            .WithName(nameof(GetMeetingWithDetailsAsync))
            .WithOpenApi();

        meetings.MapGet("/{id:int}/invitation", GetInvitationMeetingAsync)
            .WithName(nameof(GetInvitationMeetingAsync))
            .WithOpenApi();

        meetings.MapGet("/upcoming", GetUserUpcomingMeetingsAsync)
            .WithName(nameof(GetUserUpcomingMeetingsAsync))
            .WithOpenApi();

        meetings.MapGet("/pending", GetUserPendingMeetingsAsync)
            .WithName(nameof(GetUserPendingMeetingsAsync))
            .WithOpenApi();

        meetings.MapGet("/passed", GetUserPassedMeetingsAsync)
            .WithName(nameof(GetUserPassedMeetingsAsync))
            .WithOpenApi();

        meetings.MapGet("/hosted", GetUserHostedMeetingsAsync)
            .WithName(nameof(GetUserHostedMeetingsAsync))
            .WithOpenApi();

        meetings.MapGet("/attending", GetUserAttendingMeetingsAsync)
            .WithName(nameof(GetUserAttendingMeetingsAsync))
            .WithOpenApi();

        meetings.MapPost("", CreateMeetingAsync)
            .AddFluentValidationFilter()
            .WithName(nameof(CreateMeetingAsync))
            .WithOpenApi();

        meetings.MapPut("/{id:int}", UpdateMeetingAsync)
            .AddFluentValidationFilter()
            .WithName(nameof(UpdateMeetingAsync))
            .WithOpenApi();

        meetings.MapGet("/{id:int}/update", GetUpdateMeetingAsync)
            .WithName(nameof(GetUpdateMeetingAsync))
            .WithOpenApi();

        meetings.MapDelete("/{id:int}", DeleteMeetingAsync)
            .WithName(nameof(DeleteMeetingAsync))
            .WithOpenApi();

        meetings.MapDelete("/{id:int}/cancel", CancelMeetingAsync)
            .WithName(nameof(CancelMeetingAsync))
            .WithOpenApi();
    }

    private static async Task<IResult> GetAllMeetingsAsync([FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetAllMeetingsAsync();

        var meetingDtos = mapper.Map<List<MeetingDTO>>(meetings);
        return Results.Ok(meetingDtos);
    }

    private static async Task<IResult> GetMeetingByIdAsync([FromRoute] int id,
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meeting = await meetingsService.GetMeetingByIdAsync(id);
        if (meeting == null)
        {
            return Results.NotFound($"No meeting found with id: {id}");
        }

        var meetingDto = mapper.Map<MeetingDTO>(meeting);
        return Results.Ok(meetingDto);
    }

    private static async Task<IResult> GetMeetingWithDetailsAsync([FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [FromRoute] int id, 
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meeting = await meetingsService.GetMeetingWithDetailsAsync(id);
        if (meeting == null)
        {
            return Results.NotFound($"No meeting found with id: {id}");
        }

        var meetingDto = mapper.Map<MeetingDetailedDTO>(meeting, opts => opts.Items.Add(AppConstants.MapperCurrentUserIdKey, userId));
        return Results.Ok(meetingDto);
    }

    private static async Task<IResult> GetInvitationMeetingAsync([FromRoute] int id,
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meeting = await meetingsService.GetMeetingWithDetailsAsync(id);
        if (meeting == null)
        {
            return Results.NotFound($"No meeting found with id: {id}");
        }

        var meetingDto = mapper.Map<MeetingInvitationDTO>(meeting);
        return Results.Ok(meetingDto);
    }

    private static async Task<IResult> GetUserUpcomingMeetingsAsync(
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [AsParameters] QueryParams query, 
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetUpcomingMeetingsAsync(userId, new Query
        {
            SearchTerm = query.SearchTerm,
            Skip = query.Skip,
            Take = query.Take
        });

        var meetingsDto = mapper.Map<List<ListedMeetingDTO>>(meetings, opts => opts.Items.Add(AppConstants.MapperCurrentUserIdKey, userId));
        return Results.Ok(meetingsDto);
    }

    private static async Task<IResult> GetUserPendingMeetingsAsync(
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [AsParameters] QueryParams query, 
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetPendingMeetingsAsync(userId, new Query
        {
            SearchTerm = query.SearchTerm,
            Skip = query.Skip,
            Take = query.Take
        });

        var meetingsDto = mapper.Map<List<PendingMeetingDTO>>(meetings);
        return Results.Ok(meetingsDto);
    }

    private static async Task<IResult> GetUserPassedMeetingsAsync(
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [AsParameters] QueryParams query,
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetPassedMeetingsAsync(userId, new Query
        {
            SearchTerm = query.SearchTerm,
            Skip = query.Skip,
            Take = query.Take
        });

        var meetingsDto = mapper.Map<List<ListedMeetingDTO>>(meetings, opt => opt.Items.Add(AppConstants.MapperCurrentUserIdKey, userId));
        return Results.Ok(meetingsDto);
    }

    private static async Task<IResult> GetUserHostedMeetingsAsync(
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [AsParameters] QueryParams query, 
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetHostedMeetingsAsync(userId, new Query
        {
            Skip = query.Skip,
            Take = query.Take
        });

        var meetingsDto = mapper.Map<List<MeetingDTO>>(meetings);
        return Results.Ok(meetingsDto);
    }

    private static async Task<IResult> GetUserAttendingMeetingsAsync(
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [AsParameters] QueryParams query, 
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meetings = await meetingsService.GetAttendingMeetingsAsync(userId, new Query
        {
            Skip = query.Skip,
            Take = query.Take
        });

        var meetingsDto = mapper.Map<List<MeetingDTO>>(meetings);
        return Results.Ok(meetingsDto);
    }

    private static async Task<IResult> CreateMeetingAsync([FromBody] MeetingCreateDTO model,
        [FromServices] IMeetingsService meetingsService, 
        [FromServices] IMapper mapper)
    {
        var meeting = mapper.Map<Meeting>(model);
        meeting.StartTime = model.StartTime.ToUniversalTime();
        var result = await meetingsService.CreateMeetingAsync(meeting, meeting.Topics?.ToList(), meeting.Location, model.AttendeeInfos);

        var meetingDto = mapper.Map<MeetingDTO>(result);
        return Results.Ok(meetingDto);
    }

    private static async Task<IResult> UpdateMeetingAsync([FromRoute] int id, 
        [FromBody] MeetingUpdateDTO model,
        [FromServices] IMeetingsService meetingsService, 
        [FromServices] IMapper mapper)
    {
        var meeting = mapper.Map<Meeting>(model);
        meeting.Id = id;

        var result = await meetingsService.UpdateMeetingAsync(id, meeting, meeting.Topics?.ToList(), meeting.Location, model.AttendeeInfos);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetUpdateMeetingAsync([FromRoute] int id,
        [FromServices] IMeetingsService meetingsService,
        [FromServices] IMapper mapper)
    {
        var meeting = await meetingsService.GetMeetingWithDetailsAsync(id);
        if (meeting == null)
        {
            return Results.NotFound($"No meeting found with id: {id}");
        }

        var meetingDto = mapper.Map<MeetingUpdateDTO>(meeting);
        return Results.Ok(meetingDto);
    }

    private static async Task<IResult> DeleteMeetingAsync([FromRoute] int id, [FromServices] IMeetingsService meetingsService)
    {
        var result = await meetingsService.DeleteMeetingAsync(id);
        return Results.Ok(result);
    }

    private static async Task<IResult> CancelMeetingAsync([FromRoute] int id,
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [FromServices] IMeetingsService meetingsService)
    {
        var result = await meetingsService.CancelMeetingAsync(id, userId);
        return Results.Ok(result);
    }
}
