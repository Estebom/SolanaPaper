using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SolanaPaper.Data.Services
{
    public class TokenRepository
    {
        private readonly IMongoCollection<Tokens> _tokensCollection;

        public TokenRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _tokensCollection = database.GetCollection<Tokens>(mongoDBSettings.Value.CollectionName2);
        }
    }
}
