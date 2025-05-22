using Microsoft.OpenApi.Models;
using MeetingsService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MeetingsService.Api.Extensions;
using MeetingsService.Api.Helpers;
using MeetingsService.Api.Endpoints;
using MeetingsService.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MeetingsContext>(config
    => config.UseSqlServer(builder.Configuration.GetConnectionString(nameof(MeetingsContext))).UseLazyLoadingProxies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup => setup.SwaggerDoc("v1", new OpenApiInfo()
{
    Description = "Meetings catalog implementation using Minimal Api",
    Title = "MinimalMeet",
    Version = "v1",
}));

builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddValidators();
builder.AddFluentValidationEndpointFilter();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<MeetingsContext>();
    context.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapMeetingsEndpoints();
app.MapTopicsEndpoints();
app.MapLocationsEndpoints();
app.MapAttendeeInfosEndpoints();

app.Run();
