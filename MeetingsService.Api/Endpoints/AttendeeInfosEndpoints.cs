using AutoMapper;
using MeetingsService.Core.Queries;
using Microsoft.AspNetCore.Mvc;
using MinimalMeet.Common.Enums;
using CommonConstants = MinimalMeet.Common.Constants;

namespace MeetingsService.Api.Endpoints;

public static class AttendeeInfosEndpoints
{
    public static void MapAttendeeInfosEndpoints(this WebApplication app)
    {
        var attendeeInfos = app.MapGroup("api/" + GroupNames.AttendeeInfos);

        attendeeInfos.MapGet("/roles/requested", GetRequestedRolesByHostAsync)
            .WithName(nameof(GetRequestedRolesByHostAsync))
            .WithOpenApi();

        attendeeInfos.MapPost("/meetings/{meetingId:int}/join", JoinMeetingAsync)
            .WithName(nameof(JoinMeetingAsync))
            .WithOpenApi();
        
        attendeeInfos.MapPut("/{meetingId:int}/invitation/{userId:guid}/accept", AcceptInvitationAsync)
            .WithName(nameof(AcceptInvitationAsync))
            .WithOpenApi();

        attendeeInfos.MapPut("/{meetingId:int}/invitation/{userId:guid}/decline", DeclineInvitationAsync)
            .WithName(nameof(DeclineInvitationAsync))
            .WithOpenApi();

        attendeeInfos.MapPost("/{meetingId:int}/request-role", RequestRoleAsync)
            .WithName(nameof(RequestRoleAsync))
            .WithOpenApi();

        attendeeInfos.MapPut("/{meetingId:int}/role/{userId:guid}/accept", AcceptRoleRequestAsync)
            .WithName(nameof(AcceptRoleRequestAsync))
            .WithOpenApi();

        attendeeInfos.MapPut("/{meetingId:int}/role/{userId:guid}/decline", DeclineRoleRequestAsync)
            .WithName(nameof(DeclineRoleRequestAsync))
            .WithOpenApi();
    }

    private static async Task<IResult> GetRequestedRolesByHostAsync([FromHeader(Name = CommonConstants.XEndUserId)] Guid hostId,
        [AsParameters] Query query,
        [FromServices] IAttendeeInfosService attendeeService,
        [FromServices] IMapper mapper)
    {
        var requestedRoles = await attendeeService.GetRequestedRolesAsync(hostId, query);

        var requestedRolesDto = mapper.Map<List<AttendeeRoleRequestDTO>>(requestedRoles);
        return Results.Ok(requestedRolesDto);
    }

    private static async Task<IResult> AcceptRoleRequestAsync([FromRoute] int meetingId,
        [FromRoute] Guid userId,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.AcceptRoleRequestAsync(meetingId, userId);
        if (!result)
        {
            throw new ArgumentException("Could not find the attendee role request or the request is already accepted.");
        }

        return Results.Ok(result);
    }

    private static async Task<IResult> DeclineRoleRequestAsync([FromRoute] int meetingId,
        [FromRoute] Guid userId,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.DeclineRoleRequestAsync(meetingId, userId);
        if (!result)
        {
            throw new ArgumentException("Could not find the attendee role request or the request is already declined.");
        }

        return Results.Ok(result);
    }

    private static async Task<IResult> AcceptInvitationAsync([FromRoute] int meetingId,
        [FromRoute] Guid userId,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.AcceptInvitationAsync(meetingId, userId);
        if (!result)
        {
            throw new ArgumentException("Could not find the attendee invitation or the invitation is already accepted.");
        }

        return Results.Ok(result);
    }

    private static async Task<IResult> DeclineInvitationAsync([FromRoute] int meetingId,
        [FromRoute] Guid userId,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.DeclineInvitationAsync(meetingId, userId);
        if (!result)
        {
            throw new ArgumentException("Could not find the attendee invitation or the invitation is already declined.");
        }

        return Results.Ok(result);
    }

    private static async Task<IResult> RequestRoleAsync([FromRoute] int meetingId,
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [FromQuery] AttendeeRoles requestedRole,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.RequestRoleAsync(meetingId, userId, requestedRole);
        if (!result)
        {
            throw new ArgumentException("Could not find the attendee role request or the request is already requested.");
        }

        return Results.Ok(result);
    }

    private static async Task<IResult> JoinMeetingAsync([FromRoute] int meetingId,
        [FromHeader(Name = CommonConstants.XEndUserId)] Guid userId,
        [FromServices] IAttendeeInfosService attendeeService)
    {
        var result = await attendeeService.JoinMeetingAsync(meetingId, userId);
        if (!result)
        {
            throw new ArgumentException("Could not find the meeting or you're not allowed to join this meeting.");
        }

        return Results.Ok(result);
    }
}
