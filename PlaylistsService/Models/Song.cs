using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace PlaylistsService.Models
{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public int ExternalId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}