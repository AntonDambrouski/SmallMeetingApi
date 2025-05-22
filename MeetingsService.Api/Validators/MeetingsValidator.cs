using FluentValidation;
using MeetingsService.Core.Constants;

namespace MeetingsService.Api.Validators;

public class MeetingCreateValidator : AbstractValidator<MeetingCreateDTO>
{
    public MeetingCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .MinimumLength(AppConstants.MeetingTitleMinLength)
            .MaximumLength(AppConstants.MeetingTitleMaxLength);

        RuleFor(x => x.Description)
            .MinimumLength(AppConstants.MeetingDescriptionMinLength)
            .MaximumLength(AppConstants.MeetingDescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartTime).NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.HostId).NotEmpty().NotEqual(Guid.Empty);
        RuleFor(x => x.Duration).NotEmpty()
            .GreaterThanOrEqualTo(TimeSpan.FromMinutes(AppConstants.MeetingDurationMinValueInMinutes));

        RuleFor(x => x.Location).NotNull().SetValidator(new LocationValidator());
        RuleForEach(x => x.Topics).NotNull().SetValidator(new TopicsValidator());
        RuleFor(x => x.AttendeeInfos).NotEmpty();

        RuleFor(x => x.InvitationMessage)
            .MinimumLength(AppConstants.MeetingInvitationMessageMinLength)
            .MaximumLength(AppConstants.MeetingInvitationMessageMaxLength)
            .When(x => !string.IsNullOrEmpty(x.InvitationMessage));

        RuleFor(x => x.AttendeeInfos).NotEmpty();
    }
}

public class MeetingUpdateValidator : AbstractValidator<MeetingUpdateDTO>
{
    public MeetingUpdateValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .MinimumLength(AppConstants.MeetingTitleMinLength)
            .MaximumLength(AppConstants.MeetingTitleMaxLength);

        RuleFor(x => x.Description)
            .MinimumLength(AppConstants.MeetingDescriptionMinLength)
            .MaximumLength(AppConstants.MeetingDescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartTime).NotEmpty()
            .GreaterThan(DateTime.UtcNow);

        RuleFor(x => x.Duration).NotEmpty()
            .GreaterThanOrEqualTo(TimeSpan.FromMinutes(AppConstants.MeetingDurationMinValueInMinutes));

        RuleFor(x => x.Location).NotNull().SetValidator(new LocationValidator());
        RuleForEach(x => x.Topics).NotNull().SetValidator(new TopicsValidator());
        RuleFor(x => x.AttendeeInfos).NotEmpty();

        RuleFor(x => x.InvitationMessage)
            .MinimumLength(AppConstants.MeetingInvitationMessageMinLength)
            .MaximumLength(AppConstants.MeetingInvitationMessageMaxLength)
            .When(x => !string.IsNullOrEmpty(x.InvitationMessage));

        RuleFor(x => x.AttendeeInfos).NotEmpty();
    }
}
