using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Album
    {
        [Key]
        public int No { get; set; }

        public string AlbumTitle { get; set; }

        public string Artist { get; set; }

        public ICollection<Song> SongList { get; set; }
    }
}
