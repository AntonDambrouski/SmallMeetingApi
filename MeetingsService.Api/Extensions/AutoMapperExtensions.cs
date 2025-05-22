using AutoMapper;
using MeetingsService.Core.Constants;

namespace MeetingsService.Api.Extensions;

public static class AutoMapperExtensions
{
    public static Guid GetUserIdOrDefault(this ResolutionContext context)
    {
        if (context is null
            || !context.Items.TryGetValue(AppConstants.MapperCurrentUserIdKey, out var userId))
        {
            return Guid.Empty;
        }

        return (Guid) userId;
    }
}
