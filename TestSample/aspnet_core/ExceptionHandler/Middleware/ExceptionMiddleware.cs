using HandlingExceptions.Models;
using Microsoft.AspNetCore.Http.Features;

namespace HandlingExceptions.Middleware;

public class ExceptionMiddleware
{
    private const int DefaultStatusCode = StatusCodes.Status500InternalServerError;

    private readonly RequestDelegate _next;
    private PathString _exceptionHandlingPath { get; set; }
    private readonly RequestDelegate _exceptionHandling;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        _next = next;
    }

    public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory,
        string? errorHandlingPath, RequestDelegate exceptionHandling)
    {
        _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        _next = next;

        _exceptionHandlingPath = new PathString(errorHandlingPath);
        _exceptionHandling = exceptionHandling;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = DefaultStatusCode;

        var exceptionDetails = new ExceptionDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Error from middleware.",
            DetailMessage = exception.Message,
        };

        if (_exceptionHandlingPath != null &&
            _exceptionHandlingPath.HasValue &&
            _exceptionHandling is not null)
        {
            context.Request.Path = _exceptionHandlingPath;

            ClearHttpContext(context);

            // HttpContext에 예외 정보 추가
            context.Features.Set<ExceptionDetails>(exceptionDetails);
            await _exceptionHandling(context);
        }
        else
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(exceptionDetails.ToString());
        }
    }

    private static void ClearHttpContext(HttpContext context)
    {
        context.Response.Clear();

        context.SetEndpoint(endpoint: null);
        var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
        if (routeValuesFeature != null)
        {
            routeValuesFeature.RouteValues = null!;
        }
    }
}
