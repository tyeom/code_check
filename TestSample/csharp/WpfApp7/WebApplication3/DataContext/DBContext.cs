using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.DataContext
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
            //
        }

        public DbSet<Album> Album { get; set; }
        public DbSet<Song> Song { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConn"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 멀티 Key 설정
            //modelBuilder.Entity<Song>()
            //    .HasKey(p => new
            //    {
            //        p.No,
            //        p.TrackNo,
            //    });

            // Key 설정
            //modelBuilder.Entity<Song>()
            //    .HasKey(p => new
            //    {
            //        p.No,
            //    });

            // identity 설정
            //modelBuilder.Entity<Song>(p =>
            //{
            //    p.Property(c => c.No)
            //    .ValueGeneratedOnAdd();
            //});

            // 넌클러스터 인덱스 설정 [TrackNo 컬럼]
            modelBuilder.Entity<Song>()
                .HasIndex(b => b.TrackNo)
                .IsClustered(false);

            modelBuilder.Entity<Song>()
                .HasOne(p => p.Album)
                .WithMany(p => p.SongList)
                .HasForeignKey(p => p.AlbumNo)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
