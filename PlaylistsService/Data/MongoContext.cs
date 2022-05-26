using MongoDB.Driver;
using PlaylistsService.Models;

namespace PlaylistsService.Data
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; }

        public MongoContext(MongoDbSetting mongoDbSetting)
        {
            var mongoClient = new MongoClient(mongoDbSetting.ConnectionString);
            Database = mongoClient.GetDatabase(mongoDbSetting.DatabaseName);
        }
    }
}