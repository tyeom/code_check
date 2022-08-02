using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Song
    {
        [Key]
        public int No { get; set; }

        public int TrackNo { get; set; }

        public int AlbumNo { get; set; }

        [ForeignKey("AlbumNo")]
        public Album Album { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }
    }
}
