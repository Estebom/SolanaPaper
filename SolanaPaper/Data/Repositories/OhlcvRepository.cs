using Microsoft.Extensions.Options;
using SolanaPaper.Data.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Cryptography.X509Certificates;
using SolanaPaper.Data.Models.ServiceSettings;
using TradingView.Blazor.Models;

namespace SolanaPaper.Data.Repositories
{
    public class OhlcvRepository
    {
        private readonly IMongoCollection<OhlcvDTO> _OhlcvDataCollection;

        public OhlcvRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _OhlcvDataCollection = database.GetCollection<OhlcvDTO>(mongoDBSettings.Value.CollectionName3);
        }

        public async Task<List<OhlcvDTO>> GetByContactAddress(string contractAddress)
        {
            try
            {
                FilterDefinition<OhlcvDTO> filter = Builders<OhlcvDTO>.Filter.Eq("_ca", contractAddress);
                return await _OhlcvDataCollection.Find(filter).ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Create(OhlcvDTO ohlcv)
        {
            try
            {
                var existingTimes = await _OhlcvDataCollection.Find(x => x.ContactAddress == ohlcv.ContactAddress)
                      .Project(x => x.Time).ToListAsync();

                if (!existingTimes.Contains(ohlcv.Time)) 
                {
                    await _OhlcvDataCollection.InsertOneAsync(ohlcv);
                }
                else
                {
                    Console.WriteLine("No new data to insert.");
                }

                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task Create(List<OhlcvDTO> ohlcvs)
        {
            try
            {
                var existingTimes = await _OhlcvDataCollection.Find(x => x.ContactAddress == ohlcvs[0].ContactAddress)
                    .Project(x => x.Time).ToListAsync();

                var newOhlcvs = ohlcvs.Where(x => !existingTimes.Contains(x.Time)).ToList();

                if (newOhlcvs.Any())
                {
                    await _OhlcvDataCollection.InsertManyAsync(newOhlcvs);
                }
                else 
                {
                    Console.WriteLine("No new data to insert.");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //public async Task Update(string contactAddress, MarketData marketData)
        //{
        //    try
        //    {
        //        FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.Eq("_ca", contactAddress);
        //        await _marketDataCollection.ReplaceOneAsync(filter, marketData);
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //public async Task Delete(string contactAddress)
        //{
        //    try
        //    {
        //        FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.AnyEq("_ca", contactAddress);
        //        await _marketDataCollection.DeleteManyAsync(filter);
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        //public async Task Delete(List<string> contactAddresses)
        //{
        //    try
        //    {
        //        FilterDefinition<MarketData> filter = Builders<MarketData>.Filter.In("_ca", contactAddresses);
        //        await _marketDataCollection.DeleteManyAsync(filter);
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
    }
}