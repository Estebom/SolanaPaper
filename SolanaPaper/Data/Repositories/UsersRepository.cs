using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SolanaPaper.Data.Services
{
    public class UsersRepository
    {
        private readonly IMongoCollection<Users> _usersCollection;
        
        public UsersRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<Users>(mongoDBSettings.Value.CollectionName1);
        }

        public async Task<List<Users>> Get() 
        {
            return await _usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Users> Get(string username) 
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
            return await _usersCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
