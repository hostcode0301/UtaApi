using System.ComponentModel.DataAnnotations;

namespace SongsService.Models
{
    public class Song
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Name { get; set; } = null!;

        public string[] Links { get; set; } = null!;

        public string[] ArtistId { get; set; } = null!;

        public ICollection<Artist> Artists { get; set; } = null!;
    }
}