using HandlingExceptions.Middleware;

namespace HandlingExceptions.Extensions;

public static class ExceptionMiddlewareExtensions
{
    internal const string GlobalRouteBuilderKey = "__GlobalEndpointRouteBuilder";
    internal const string UseRoutingKey = "__UseRouting";

    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app, string? errorHandlingPath = null)
    {
        if(string.IsNullOrWhiteSpace(errorHandlingPath) is true)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
        else
        {
            app.Use(next =>
            {
                var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
                return new ExceptionMiddleware(next, loggerFactory, errorHandlingPath, Reroute(app, errorHandlingPath!, next)).InvokeAsync;
            });
        }
    }

    internal static RequestDelegate Reroute(IApplicationBuilder app, string errorHandlingPath, RequestDelegate next)
    {
        var builder = app.New();

        Func<IApplicationBuilder, IApplicationBuilder> useRoutingFunc =
            (Func<IApplicationBuilder, IApplicationBuilder>)app.Properties[UseRoutingKey];

        builder.Properties[GlobalRouteBuilderKey] = app.Properties[GlobalRouteBuilderKey];
        
        useRoutingFunc(builder);

        // ExceptionMiddleware 호출 미들웨어 적용
        builder.Run(next);
        return builder.Build();
    }
}
