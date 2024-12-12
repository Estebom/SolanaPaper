using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class MarketData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("_ca")]
        [JsonPropertyName("_ca")]
        public string ContactAddress { get; set; } = null!;

        [BsonElement("timestamp")]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("marketPrice")]
        [JsonPropertyName("marketPrice")]
        public double MarketPrice { get; set; }
    }
}
