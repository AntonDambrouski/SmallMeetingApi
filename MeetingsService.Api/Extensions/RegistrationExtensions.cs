using FluentValidation;
using MeetingsService.Api.Validators;
using MeetingsService.Core.Interfaces;
using MeetingsService.Core.Services;
using MeetingsService.Infrastructure.Repositories;

namespace MeetingsService.Api.Extensions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMeetingsRepository, MeetingsRepository>();
        services.AddScoped<IAttendeeInfosRepository, AttendeeInfosRepository>();
        services.AddScoped<ILocationsRepository, LocationsRepository>();
        services.AddScoped<ITopicsRepository, TopicsRepository>();
        services.AddScoped<IAttendeeInfosRepository, AttendeeInfosRepository>();
        services.AddScoped<IFeedbacksRepository, FeedbacksRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IMeetingsService, MeetingService>();
        services.AddScoped<IAttendeeInfosService, AttendeeInfosService>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<MeetingCreateDTO>, MeetingCreateValidator>();
        services.AddScoped<IValidator<MeetingUpdateDTO>, MeetingUpdateValidator>();
        services.AddScoped<IValidator<LocationDTO>, LocationValidator>();
        services.AddScoped<IValidator<TopicDTO>, TopicsValidator>();

        return services;
    }
}
