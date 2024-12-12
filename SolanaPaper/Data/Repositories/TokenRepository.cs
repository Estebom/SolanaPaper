using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
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

        public async Task<List<Tokens>> Get()
        {
            return await _tokensCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Tokens> GetByContactAddress(string contactAddress)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            return await _tokensCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Tokens>> GetBySymbol(string symbol)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.In("symbol", symbol);
            return await _tokensCollection.Find(filter).ToListAsync();
        }

        public async Task Create(Tokens token)
        {
            await _tokensCollection.InsertOneAsync(token);
            return;
        }

        public async Task UpdateMintAuthority(string contactAddress, string mintAuthority)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Set("mintAuthority", mintAuthority);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task AddHolder(string contactAddress, Holder holder)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.AddToSet<Holder>("holders", holder);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task AddHolder(string contactAddress, List<Holder> holder)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.AddToSet<List<Holder>>("holders", holder);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task RemoveHolder(string contactAddress, Holder holder)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Pull<Holder>("holders", holder);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task RemoveHolder(string contactAddress, List<Holder> holder)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Pull<List<Holder>>("holders", holder);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }

        public async Task UpdateSocials(string contactAddress, Socials socials)
        {
            FilterDefinition<Tokens> filter = Builders<Tokens>.Filter.Eq("_ca", contactAddress);
            UpdateDefinition<Tokens> update = Builders<Tokens>.Update.Set("socials", socials);
            await _tokensCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}
