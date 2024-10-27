using System.Net;

namespace DotNet8.POS.ApiGateway.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An unhandled exception occurred while processing the request.");

        context.Response.ContentType = "application/json";

        var statusCode = (int)HttpStatusCode.InternalServerError;

        if (ex is ArgumentNullException)
        {
            statusCode = (int)HttpStatusCode.BadRequest;
        }
        else if (ex is UnauthorizedAccessException)
        {
            statusCode = (int)HttpStatusCode.Unauthorized;
        }

        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = "An error occurred while processing your request.",
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}