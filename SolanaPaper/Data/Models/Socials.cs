using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace SolanaPaper.Data.Models
{
    public class Socials
    {
        [BsonElement("websiteLink")]
        [JsonPropertyName("websiteLink")]
        public string? WebsiteLink { get; set; }

        [BsonElement("twitterLink")]
        [JsonPropertyName("twitterLink")]
        public string? TwitterLink { get; set; }
        
        [BsonElement("telegramLink")]
        [JsonPropertyName("telegramLink")]
        public string? TelegramLink { get; set; }
    }
}
