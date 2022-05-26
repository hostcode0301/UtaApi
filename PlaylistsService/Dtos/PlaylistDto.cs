namespace PlaylistsService.Dtos
{
    public class PlaylistReadDto
    {
        public string Id { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public ICollection<SongReadDto> Songs { get; set; } = null!;
    }

    public class PlaylistCreateDto
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}