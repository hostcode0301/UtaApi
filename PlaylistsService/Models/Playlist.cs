using MongoDB.Bson.Serialization.Attributes;

namespace PlaylistsService.Models
{
    public class Playlist
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [BsonIgnoreIfNull]
        public ICollection<MongoDB.Bson.ObjectId> SongIds { get; set; } = null!;

        [BsonIgnoreIfNull]
        public ICollection<Song> Songs { get; set; } = null!;
    }
}