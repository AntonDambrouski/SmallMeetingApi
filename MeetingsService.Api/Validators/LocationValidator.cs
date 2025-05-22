using FluentValidation;
using MeetingsService.Core.Constants;

namespace MeetingsService.Api.Validators;

public class LocationValidator : AbstractValidator<LocationDTO>
{
    public LocationValidator()
    {
        RuleFor(x => x.Place).NotEmpty()
            .MinimumLength(AppConstants.LocationPlaceMinLength)
            .MaximumLength(AppConstants.LocationPlaceMaxLength);
    }
}
