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

        [BsonElement("price")]
        [JsonPropertyName("price")]
        public double Price { get; set; }

        [BsonElement("marketCap")]
        [JsonPropertyName("marketCap")]
        public double MarketCap { get; set; }

        [BsonElement("supply")]
        [JsonPropertyName("supply")]
        public double CurrentSupply { get; set; }

        [BsonElement("timestamp")]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
