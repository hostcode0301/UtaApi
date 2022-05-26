using ArtistsService.Models;
using MongoDB.Driver;

namespace ArtistsService.Data
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(MongoDbSetting monogoDbSetting)
        {
            var client = new MongoClient(monogoDbSetting.ConnectionString);
            Database = client.GetDatabase(monogoDbSetting.DatabaseName);
        }
    }
}