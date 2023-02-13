namespace Rate_limiting_Example
{
    using Microsoft.AspNetCore.RateLimiting;
    using Rate_limiting_Example.Services;
    using System.Globalization;
    using System.Net;
    using System.Threading.RateLimiting;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // GetTimeService 서비스 등록
            builder.Services.AddScoped<IGetTimeService, GetTimeService>();

            // Fixed window limit 알고리즘 방식으로 속도 제한 처리
            builder.Services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "LimiterPolicy", options =>
            {
                // 요청 허용 갯수 : 1
                options.PermitLimit = 1;
                // 창 이동시간 10초 [10초 동안 최대 1개의 요청만 처리 가능]
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                // 제한시 3개의 요청만 대기열에 추가
                options.QueueLimit = 3;
            }));



            // Sliding window limit 알고리즘 방식으로 속도 제한 처리
            //builder.Services.AddRateLimiter(_ => _
            //.AddSlidingWindowLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // 요청 허용 갯수 : 100
            //    options.PermitLimit = 100;
            //    // 창 이동시간 30초
            //    options.Window = TimeSpan.FromSeconds(30);
            //    // 창 분할 세그먼트 갯수
            //    options.SegmentsPerWindow = 3;  // 1개의 세그먼트 : 30s / 3
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // 제한시 3개의 요청만 대기열에 추가
            //    options.QueueLimit = 3;
            //}));



            // Token bucket limit 알고리즘 방식으로 속도 제한 처리
            //builder.Services.AddRateLimiter(_ => _
            //.AddTokenBucketLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // 요청 허용 갯수 : 1
            //    options.TokenLimit = 1;
            //    // 토큰 보충 개수 : 1
            //    options.TokensPerPeriod = 1;
            //    // 토큰 보충 시간 : 1초
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // 제한시 3개의 요청만 대기열에 추가
            //    options.QueueLimit = 3;
            //}));



            // Concurrency limit 알고리즘 방식으로 속도 제한 처리
            //builder.Services.AddRateLimiter(_ => _
            //.AddConcurrencyLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // 요청 허용 갯수 : 1
            //    options.PermitLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // 제한시 3개의 요청만 대기열에 추가
            //    options.QueueLimit = 3;
            //}));



            // 사용자 및 IPAddress로 속도 제한 설정
            // 환경설정 'MyRateLimit' 섹션 내용 의존성 주입 서비스 등록
            builder.Services.Configure<MyRateLimitOptions>(
                builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit));

            // 환경설정 'MyRateLimit' 섹션 내용 바인딩
            var myOptions = new MyRateLimitOptions();
            builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

            builder.Services.AddRateLimiter(limiterOptions =>
            {
                // 요청이 속도 제한 초과시 호출되는 OnRejected 콜백
                limiterOptions.OnRejected = (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;


                    return ValueTask.CompletedTask;
                };

                limiterOptions.AddPolicy<string, CustomRateLimiterPolicy>("customLimiter");

                // 글로벌 속도 제한 설정
                //limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                //{
                //    // 요청 IPAddress
                //    IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

                //    // 요청된 IPAddress가 루프백이 아닌 경우
                //    if (IPAddress.IsLoopback(remoteIpAddress!) == false)
                //    {
                //        // IPAddress에 대해 속도 제한 설정 [Token bucket limit 알고리즘 적용]
                //        return RateLimitPartition.GetFixedWindowLimiter
                //        (remoteIpAddress!, _ =>
                //            new FixedWindowRateLimiterOptions
                //            {
                //                // 요청 허용 갯수 : 1
                //                PermitLimit = myOptions.PermitLimit,
                //                //    // 창 이동시간 10초 [10초 동안 최대 1개의 요청만 처리 가능]
                //                Window = TimeSpan.FromSeconds(myOptions.Window),
                //                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                //                //    // 제한시 3개의 요청만 대기열에 추가
                //                QueueLimit = myOptions.QueueLimit,
                //            });
                //    }

                //    // 루프백 IPAddress는 속도 제한 없음.
                //    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                //});
            });



            var app = builder.Build();
            // 속도 제한 사용
            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}