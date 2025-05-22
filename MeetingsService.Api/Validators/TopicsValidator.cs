using FluentValidation;
using MeetingsService.Core.Constants;

namespace MeetingsService.Api.Validators;

public class TopicsValidator : AbstractValidator<TopicDTO>
{
    public TopicsValidator()
    {
        RuleFor(x => x.Name).NotEmpty()
            .MinimumLength(AppConstants.TopicNameMinLength)
            .MaximumLength(AppConstants.TopicNameMaxLength);
    }
}
