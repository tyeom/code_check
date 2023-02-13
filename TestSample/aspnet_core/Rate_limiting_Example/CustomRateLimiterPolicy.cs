namespace Rate_limiting_Example
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.RateLimiting;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading;
    using System.Threading.RateLimiting;
    using System.Threading.Tasks;

    public class CustomRateLimiterPolicy : IRateLimiterPolicy<string>
    {
        private readonly MyRateLimitOptions _options;
        private Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;

        public CustomRateLimiterPolicy(IOptions<MyRateLimitOptions> options)
        {
            // 요청이 속도 제한 초과시 호출되는 OnRejected 콜백
            _onRejected = (ctx, token) =>
            {
                ctx.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return ValueTask.CompletedTask;
            };
            _options = options.Value;
        }

        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => _onRejected;

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            // 특정 유저 정보 (Header, JWT 등)를 읽어서 속도 제한 설정 처리

            var username = "anonymous user";
            if (httpContext.User.Identity?.IsAuthenticated is true)
            {
                username = httpContext.User.ToString()!;
            }

            return RateLimitPartition.GetFixedWindowLimiter(string.Empty,
                _ => new FixedWindowRateLimiterOptions
                {
                    // 요청 허용 갯수 : 1
                    PermitLimit = _options.PermitLimit,
                    //    // 창 이동시간 10초 [10초 동안 최대 1개의 요청만 처리 가능]
                    Window = TimeSpan.FromSeconds(_options.Window),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    //    // 제한시 3개의 요청만 대기열에 추가
                    QueueLimit = _options.QueueLimit,
                });
        }
    }
}
