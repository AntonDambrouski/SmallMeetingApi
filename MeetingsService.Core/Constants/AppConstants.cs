namespace MeetingsService.Core.Constants;

public class AppConstants
{
    public const int MeetingDescriptionMaxLength = 500;
    public const int MeetingDescriptionMinLength = 10;
    public const int MeetingTitleMinLength = 5;
    public const int MeetingTitleMaxLength = 100;
    public const int TopicNameMaxLength = 50;
    public const int TopicNameMinLength = 2;
    public const int LocationPlaceMinLength = 3;
    public const int LocationPlaceMaxLength = 100;
    public const int MeetingInvitationMessageMaxLength = 500;
    public const int MeetingInvitationMessageMinLength = 10;
    public const double MeetingDurationMinValueInMinutes = 15;

    public const string MapperCurrentUserIdKey = "CurrentUserId";
}