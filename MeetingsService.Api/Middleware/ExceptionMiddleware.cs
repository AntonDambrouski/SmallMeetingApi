using MeetingsService.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeetingsService.Api.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Message: {message}.", ex.Message);

            int statusCode = (int)HttpStatusCode.InternalServerError;
            var details = "Internal Server Error";
            switch (ex)
            {
                case ArgumentException or ArgumentNullException:
                    details = ex.Message;
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case DomainException:
                    details = ex.Message;
                    statusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "An error occurred",
                Detail = details,
                Status = statusCode
            });
        }
    }
}
