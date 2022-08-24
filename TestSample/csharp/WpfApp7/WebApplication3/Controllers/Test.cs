using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.DataContext;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("Test")]
    public class Test : ControllerBase
    {
        private readonly DBContext _db;

        public Test(DBContext db)
        {
            _db = db;
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            Album album = new Album();
            album.Artist = $"아이유";
            album.AlbumTitle = "아이유 앨범";
            album.SongList = new List<Song>();

            foreach (var idx in Enumerable.Range(1, 5))
            {
                album.SongList.Add(new Song() { Album = album, Title = $"제목{idx}", Genre = String.Empty, TrackNo = idx });

                _db.Album.Add(album);
            }
            _db.SaveChanges();

            return Ok("success");
        }

        [HttpGet("First")]
        public async Task<Album> GetFirst()
        {
            var album = await _db.Album.Include("SongList").FirstOrDefaultAsync();

            return album;
        }

        //[HttpGet("First2")]
        //public async Task<Album> GetFirst2()
        //{
        //    var albumObj = await (from album in _db.Album
        //                    join song in _db.Song on album.No equals song.AlbumNo
        //                    select new
        //                    {
        //                        No = album.No,
        //                        AlbumTitle = album.AlbumTitle,
        //                        Artist = album.Artist,
        //                        SongList.
        //                    }).FirstOrDefaultAsync();


        //    return albumObj;
        //}

        [HttpGet("SlowTest")]
        public async Task<string> SlowTest(CancellationToken token)
        {
            Console.WriteLine("Starting to do slow work");

            for (var i = 0; i < 10; i++)
            {
                if(token.IsCancellationRequested)
                {
                    Console.WriteLine("Cancel");
                    token.ThrowIfCancellationRequested();
                }
                
                Thread.Sleep(1000);
            }

            // slow async action, e.g. call external api
            //await Task.Delay(10_000, token);

            var message = "Finished slow delay of 10 seconds.";

            return message;
        }
    }
}