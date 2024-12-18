using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Models
{
    public class OhlcvDTO
    {

            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? Id { get; set; }

            [BsonElement("_ca")]
            [JsonPropertyName("_ca")]
            public string ContactAddress { get; set; }
            public DateTime Time { get; set; }
            public decimal Open { get; set; }
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Close { get; set; }
            public decimal Volume { get; set; }


            public decimal DisplayPrice
            {
                get => Close;
            }
        
    }
}
