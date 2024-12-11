using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;


namespace SolanaPaper.Data.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Users> _usersCollection;


        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) 
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);

            UsersService _usersService = new UsersService(this, database);

            _usersCollection = database.GetCollection<Users>(mongoDBSettings.Value.CollectionName);
        }

        public async Task CreateAsync(Users users) 
        {
            await _usersCollection.InsertOneAsync(users);
            return;
        }
    }
}
