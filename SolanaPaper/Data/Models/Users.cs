using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("_user")]
        [JsonPropertyName("_user")]
        public string Username { get; set; } = null!;

        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [BsonElement("stats")]
        [JsonPropertyName("stats")]
        public Stats Stats { get; set; } = null!;

        [BsonElement("holdings")]
        [JsonPropertyName("holdings")]
        public string[] Holdings { get; set; } = null!;
    }
}
