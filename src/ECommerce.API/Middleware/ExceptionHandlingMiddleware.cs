using System.Net;
using System.Text.Json;
using ECommerce.Models;
using ECommerce.Models.Exceptions;

namespace ECommerce.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var apiResponse = exception switch
        {
            NotFoundException => ApiResponse<object>.Error(exception.Message),
            BadRequestException badRequest => ApiResponse<object>.Error(exception.Message, 
                badRequest.ValidationErrors),
            UnauthorizedException => ApiResponse<object>.Error(exception.Message),
            ForbiddenException => ApiResponse<object>.Error(exception.Message),
            DomainException => ApiResponse<object>.Error(exception.Message),
            _ => ApiResponse<object>.Error("An unexpected error occurred")
        };

        response.StatusCode = exception switch
        {
            NotFoundException => (int)HttpStatusCode.NotFound,
            BadRequestException => (int)HttpStatusCode.BadRequest,
            UnauthorizedException => (int)HttpStatusCode.Unauthorized,
            ForbiddenException => (int)HttpStatusCode.Forbidden,
            DomainException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        _logger.LogError(exception, "An error occurred: {Message}", exception.Message);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await response.WriteAsync(JsonSerializer.Serialize(apiResponse, options));
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
