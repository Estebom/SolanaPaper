using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.CompilerServices;

namespace SolanaPaper.Data.Repositories
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
            try
            {
                return await _usersCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Users> GetByUsername(string username) 
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
                return await _usersCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Users> GetByEmail(string email)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("email", email);
                return await _usersCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Create(Users user) 
        {
            try
            {
                await _usersCollection.InsertOneAsync(user);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateStats(string username, Stats stats)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
                UpdateDefinition<Users> update = Builders<Users>.Update.Set("stats", stats);
                await _usersCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddToHoldings(string user, string holding)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", user);
                UpdateDefinition<Users> update = Builders<Users>.Update.AddToSet("holdings", holding);
                await _usersCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //TODO FIX THHIS
        public async Task AddToHoldings(string user, List<string> holdings)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", user);
                UpdateDefinition<Users> update = Builders<Users>.Update.AddToSetEach("holdings", holdings);
                await _usersCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveFromHoldings(string user, string holding)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", user);
                UpdateDefinition<Users> update = Builders<Users>.Update.Pull<string>("holdings", holding);
                await _usersCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveFromHoldings(string user, List<string> holdings)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", user);
                UpdateDefinition<Users> update = Builders<Users>.Update.PullAll("holdings", holdings);
                await _usersCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public async Task Delete(string username)
        {
            try
            {
                FilterDefinition<Users> filter = Builders<Users>.Filter.Eq("_user", username);
                await _usersCollection.DeleteOneAsync(filter);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
