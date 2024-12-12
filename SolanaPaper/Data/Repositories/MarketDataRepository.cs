using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SolanaPaper.Data.Services
{
    public class MarketDataRepository
    {
        private readonly IMongoCollection<MarketData> _marketDataCollection;

        public MarketDataRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _marketDataCollection = database.GetCollection<MarketData>(mongoDBSettings.Value.CollectionName3);
        }
    }
}