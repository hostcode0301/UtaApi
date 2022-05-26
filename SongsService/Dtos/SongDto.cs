namespace SongsService.Dtos
{
    public class SongReadDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string[] Links { get; set; } = null!;

        public string[] ArtistId { get; set; } = null!;

        public ICollection<ArtistReadDto> Artists { get; set; } = null!;
    }

    public class SongCreateDto
    {
        public string Name { get; set; } = null!;

        public string[] Links { get; set; } = null!;

        public string[] ArtistId { get; set; } = null!;
    }

    public class SongUpdateDto
    {
        public string? Name { get; set; }
    }

    public class SongPublishDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Event { get; set; } = null!;
    }
}