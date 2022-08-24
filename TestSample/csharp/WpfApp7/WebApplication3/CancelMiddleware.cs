using Microsoft.Extensions.Configuration;

namespace WebApplication3
{
    public class CancelMiddleware
    {
        private readonly RequestDelegate _next;

        public CancelMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Request was cancelled");
                context.Response.StatusCode = 409;
            }
        }
    }
}
