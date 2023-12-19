using EF_Test.Entity;
using EF_Test.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EF_Test.DataContext
{
    public class EFTest_DBContext : DbContext
    {
        public EFTest_DBContext()
        {
            //
        }

        public EFTest_DBContext(DbContextOptions<EFTest_DBContext> options)
            : base(options)
        {
        }

        public DbSet<RealizedPlEntity> RealizedPlEntity { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .Build();

            options.UseSqlServer(configuration.GetConnectionString("MsSql_Debug"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 사용자 정의 SQL함수 매핑 정의 사용 (HasTranslation)
            if (Database.IsSqlServer())
            {
                modelBuilder.AddSqlDateOnlyConvertFunction();
            }

            //modelBuilder
            //    .Entity<RealizedPlEntity>()
            //    .Property(c => c.CreateDate)
            //    .HasConversion(c => c.ToString("yyyyMMdd"),
            //                   c => DateOnly.ParseExact(c, "yyyyMMdd"));
        }
    }
}
