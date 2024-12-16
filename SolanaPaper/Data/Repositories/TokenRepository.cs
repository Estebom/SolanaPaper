using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using SolanaPaper.Data.Models.ServiceSettings;

namespace SolanaPaper.Data.Repositories
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

        public async Task<List<Tokens>> Get()
        {
            try
            {
                return await _tokensCollection.Find(new BsonDocument()).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Tokens> GetByContactAddress(string contactAddress)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                return await _tokensCollection.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<List<Tokens>> GetBySymbol(string symbol)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.AnyEq("symbol", symbol);
                return await _tokensCollection.Find(filter).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Create(Tokens token)
        {
            try
            {
                await _tokensCollection.InsertOneAsync(token);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateMintAuthority(string contactAddress, string mintAuthority)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Set("mintAuthority", mintAuthority);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddHolder(string contactAddress, Holder holder)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.AddToSet("holders", holder);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddHolder(string contactAddress, List<Holder> holders)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.AddToSetEach("holders", holders);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveHolder(string contactAddress, Holder holder)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Pull("holders", holder);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task RemoveHolder(string contactAddress, List<Holder> holders)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.PullAll("holders", holders);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task UpdateSocials(string contactAddress, Socials socials)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Set("socials", socials);
                await _tokensCollection.UpdateOneAsync(filter, update);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Delete(string contactAddress)
        {
            try
            {
                FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
                await _tokensCollection.DeleteOneAsync(filter);
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
