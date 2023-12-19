
using EF_Test.DataContext;
using EF_Test.Repositories;
using EF_Test.Services;
using Microsoft.EntityFrameworkCore;

namespace EF_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var dbContext = new EFTest_DBContext();
            dbContext.Database.Migrate();

            // Add services to the container.
            builder.Services.AddDbContext<EFTest_DBContext>(options =>
            {
                options.UseSqlServer($"{builder.Configuration.GetConnectionString("MsSql_Debug")}");
            });

            builder.Services.AddScoped<RevenueRepository>();
            builder.Services.AddScoped<IRevenueService, RevenueService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
