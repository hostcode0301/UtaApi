using MongoDB.Bson.Serialization.Attributes;

namespace ArtistsService.Models
{
    public class Artist
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Name { get; set; } = null!;

        public string Gender { get; set; } = null!;

    }
}