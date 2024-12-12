using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;

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

        public async Task<List<MarketData>> GetByContactAddress(string contractAddress)
        {
            FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.Eq("_ca", contractAddress);
            return await _marketDataCollection.Find(filter).ToListAsync();
        }

        public async Task Create(MarketData marketData)
        {
            await _marketDataCollection.InsertOneAsync(marketData);
            return;
        }

        public async Task Create(List<MarketData> marketData)
        {
            await _marketDataCollection.InsertManyAsync(marketData);
            return;
        }

        public async Task Update(string contactAddress, MarketData marketData)
        {
            FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.Eq("_ca", contactAddress);
            await _marketDataCollection.ReplaceOneAsync(filter, marketData);
            return;
        }

        public async Task Delete(string contactAddress)
        {
            FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.Eq("_ca", contactAddress);
            await _marketDataCollection.DeleteOneAsync(filter);
            return;
        }

        public async Task Delete(List<string> contactAddresses)
        {
            FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.In("_ca", contactAddresses);
            await _marketDataCollection.DeleteManyAsync(filter);
            return;
        }
    }
}