using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class Stats
    {
        [BsonElement("gains")]
        [JsonPropertyName("gains")]
        public double Gains { get; set; }

        [BsonElement("losses")]
        [JsonPropertyName("losses")]
        public double Losses { get; set; }

        [BsonElement("winLoseRatio")]
        [JsonPropertyName("winLoseRatio")]
        public double Ratio {get; set;}

        [BsonElement("rugCount")]
        [JsonPropertyName("rugCount")]
        public int RugCount { get; set; }
    }
}
