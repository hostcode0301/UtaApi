namespace ArtistsService.Models
{
    public class MongoDbSetting
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;
    }
}