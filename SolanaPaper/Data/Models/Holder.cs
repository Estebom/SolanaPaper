using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class Holder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("walletKey")]
        [JsonPropertyName("walletKey")]
        public string WalletKey { get; set; } = null!;

        [BsonElement("tokensHeld")]
        [JsonPropertyName("tokensHeld")]
        public int TokensHeld { get; set; }

    }
}
