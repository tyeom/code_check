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
            
            // GetTimeService ���� ���
            builder.Services.AddScoped<IGetTimeService, GetTimeService>();

            // Fixed window limit �˰��� ������� �ӵ� ���� ó��
            builder.Services.AddRateLimiter(_ => _
            .AddFixedWindowLimiter(policyName: "LimiterPolicy", options =>
            {
                // ��û ��� ���� : 1
                options.PermitLimit = 1;
                // â �̵��ð� 10�� [10�� ���� �ִ� 1���� ��û�� ó�� ����]
                options.Window = TimeSpan.FromSeconds(10);
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                // ���ѽ� 3���� ��û�� ��⿭�� �߰�
                options.QueueLimit = 3;
            }));



            // Sliding window limit �˰��� ������� �ӵ� ���� ó��
            //builder.Services.AddRateLimiter(_ => _
            //.AddSlidingWindowLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // ��û ��� ���� : 100
            //    options.PermitLimit = 100;
            //    // â �̵��ð� 30��
            //    options.Window = TimeSpan.FromSeconds(30);
            //    // â ���� ���׸�Ʈ ����
            //    options.SegmentsPerWindow = 3;  // 1���� ���׸�Ʈ : 30s / 3
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // ���ѽ� 3���� ��û�� ��⿭�� �߰�
            //    options.QueueLimit = 3;
            //}));



            // Token bucket limit �˰��� ������� �ӵ� ���� ó��
            //builder.Services.AddRateLimiter(_ => _
            //.AddTokenBucketLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // ��û ��� ���� : 1
            //    options.TokenLimit = 1;
            //    // ��ū ���� ���� : 1
            //    options.TokensPerPeriod = 1;
            //    // ��ū ���� �ð� : 1��
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(1);
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // ���ѽ� 3���� ��û�� ��⿭�� �߰�
            //    options.QueueLimit = 3;
            //}));



            // Concurrency limit �˰��� ������� �ӵ� ���� ó��
            //builder.Services.AddRateLimiter(_ => _
            //.AddConcurrencyLimiter(policyName: "LimiterPolicy", options =>
            //{
            //    // ��û ��� ���� : 1
            //    options.PermitLimit = 1;
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    // ���ѽ� 3���� ��û�� ��⿭�� �߰�
            //    options.QueueLimit = 3;
            //}));



            // ����� �� IPAddress�� �ӵ� ���� ����
            // ȯ�漳�� 'MyRateLimit' ���� ���� ������ ���� ���� ���
            builder.Services.Configure<MyRateLimitOptions>(
                builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit));

            // ȯ�漳�� 'MyRateLimit' ���� ���� ���ε�
            var myOptions = new MyRateLimitOptions();
            builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

            builder.Services.AddRateLimiter(limiterOptions =>
            {
                // ��û�� �ӵ� ���� �ʰ��� ȣ��Ǵ� OnRejected �ݹ�
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

                // �۷ι� �ӵ� ���� ����
                //limiterOptions.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                //{
                //    // ��û IPAddress
                //    IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

                //    // ��û�� IPAddress�� �������� �ƴ� ���
                //    if (IPAddress.IsLoopback(remoteIpAddress!) == false)
                //    {
                //        // IPAddress�� ���� �ӵ� ���� ���� [Token bucket limit �˰��� ����]
                //        return RateLimitPartition.GetFixedWindowLimiter
                //        (remoteIpAddress!, _ =>
                //            new FixedWindowRateLimiterOptions
                //            {
                //                // ��û ��� ���� : 1
                //                PermitLimit = myOptions.PermitLimit,
                //                //    // â �̵��ð� 10�� [10�� ���� �ִ� 1���� ��û�� ó�� ����]
                //                Window = TimeSpan.FromSeconds(myOptions.Window),
                //                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                //                //    // ���ѽ� 3���� ��û�� ��⿭�� �߰�
                //                QueueLimit = myOptions.QueueLimit,
                //            });
                //    }

                //    // ������ IPAddress�� �ӵ� ���� ����.
                //    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                //});
            });



            var app = builder.Build();
            // �ӵ� ���� ���
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