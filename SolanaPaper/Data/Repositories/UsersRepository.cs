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

        public async Task<Users> GetByUsername(string username) 
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
            return await _usersCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Users>> GetByEmail(string email) 
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("email", email);
            return await _usersCollection.Find(filter).ToListAsync();
        }

        public async Task Create(Users user) 
        {
            await _usersCollection.InsertOneAsync(user);
            return;
        }

        public async Task UpdateStats(string username, Stats stats)
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
            UpdateDefinition<Users> update = Builders<Users>.Update.Set("stats", stats);
            await _usersCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task AddToHoldings(string user, string holding) 
        {
            FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", user);
           UpdateDefinition<Users> update = Builders<Users>.Update.Push("holdings", holding);
        }
    }
}
