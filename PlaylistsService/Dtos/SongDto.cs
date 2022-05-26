namespace PlaylistsService.Dtos
{
    public class SongReadDto
    {
        public string Id { get; set; } = null!;

        public int ExternalId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }

    public class SongCreateDto
    {
        public HashSet<int> ExternalIds { get; set; } = null!;
    }

    public class SongUpdateEventDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}