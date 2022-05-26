namespace SongsService.Dtos
{
    public class ArtistReadDto
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string Gender { get; set; } = null!;
    }
}