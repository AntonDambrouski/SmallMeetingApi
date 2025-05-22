using AutoMapper;
using MeetingsService.Api.Extensions;
using MinimalMeet.Common.Enums;

namespace MeetingsService.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        //Meetings
        CreateMap<MeetingCreateDTO, Meeting>()
            .ForMember(x => x.AttendeeInfos, o => o.Ignore());

        CreateMap<MeetingUpdateDTO, Meeting>()
            .ForMember(x => x.AttendeeInfos, o => o.Ignore());

        CreateMap<Meeting, MeetingBaseDTO>()
            .ForMember(dest => dest.MeetingId, opt => opt.MapFrom(src => src.Id));

        CreateMap<Meeting, MeetingDTO>()
           .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location.Place))
           .IncludeBase<Meeting, MeetingBaseDTO>();

        CreateMap<Meeting, ListedMeetingDTO>()
            .ForMember(dest => dest.IsUserHosting, opt => opt
                .MapFrom((src, _, _, context) => src.HostId == context.GetUserIdOrDefault()))
            .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.Topics.Select(x => x.Name).ToList()))
            .IncludeBase<Meeting, MeetingDTO>();

        CreateMap<PendingMeeting, PendingMeetingDTO>()
            .IncludeBase<Meeting, MeetingDTO>();

        CreateMap<Meeting, MeetingDetailedDTO>()
            .ForMember(dest => dest.CurrentUserAttendeeInfo, opt => opt
                .MapFrom(GetUserAttendeeInfo))
            .ForMember(dest => dest.ParticipantIds, opt => opt.MapFrom(src =>
                src.AttendeeInfos.Where(x => x.Role == AttendeeRoles.Participant).Select(x => x.UserId).ToList()))
            .ForMember(dest => dest.PresenterIds, opt => opt.MapFrom(src =>
                src.AttendeeInfos.Where(x => x.Role == AttendeeRoles.Presenter).Select(x => x.UserId).ToList()))
            .ForMember(dest => dest.IsCurrentUserHost,
                opt => opt.MapFrom((src, _, _, context) => src.HostId == context.GetUserIdOrDefault()))
            .IncludeBase<Meeting, MeetingBaseDTO>();

        CreateMap<Meeting, MeetingInvitationDTO>();

        CreateMap<Meeting, MeetingUpdateDTO>()
            .ForMember(src => src.AttendeeInfos, opt => opt
                .MapFrom(dest => dest.AttendeeInfos.ToDictionary(x => x.UserId, x => x.Role)));

        //Topics
        CreateMap<Topic, TopicDTO>();
        CreateMap<TopicDTO, Topic>();

        //Locations
        CreateMap<Location, LocationDTO>();
        CreateMap<LocationDTO, Location>();

        //AttendeeInfos
        CreateMap<AttendeeInfo, AttendeeRoleRequestDTO>()
            .ForMember(x => x.MeetingTitle, o => o.MapFrom(m => m.Meeting.Title))
            .ForMember(x => x.RequestedRole, o => o.MapFrom(m => m.RequestedRole.ToString()));

        CreateMap<AttendeeInfo, AttendeeInfoDTO>();
    }

    private static AttendeeInfoDetailedDTO GetUserAttendeeInfo(Meeting src, MeetingDetailedDTO dest, AttendeeInfoDetailedDTO _, ResolutionContext context)
    {
        var userId = context.GetUserIdOrDefault();
        if (userId != src.HostId)
        {
            var attendeeInfo = src.AttendeeInfos.FirstOrDefault(x => x.UserId == userId);
            return attendeeInfo == null ? null : new AttendeeInfoDetailedDTO
            {
                UserId = attendeeInfo.UserId,
                Role = attendeeInfo.Role,
                RequestedRole = attendeeInfo.RequestedRole,
                Status = attendeeInfo.Status,
            };
        }

        return null;
    }
}
