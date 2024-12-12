using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class Tokens
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("_ca")]
        [JsonPropertyName("_ca")]
        public string ContactAddress { get; set; } = null!;

        [BsonElement("mintAuthority")]
        [JsonPropertyName("mintAuthority")]
        public string MintAuthority { get; set; } = null!;

        [BsonElement("dev")]
        [JsonPropertyName("dev")]
        public string Creator { get; set; } = null!;

        [BsonElement("mintDate")]
        [JsonPropertyName("mintDate")]
        public DateTime MintDate { get; set; }

        [BsonElement("holders")]
        [JsonPropertyName("holders")]
        public List<Holder> Holders { get; set; } = null!;

        [BsonElement("socials")]
        [JsonPropertyName("socials")]
        public Socials SocialLinks { get; set; } = null!;
    }
}
