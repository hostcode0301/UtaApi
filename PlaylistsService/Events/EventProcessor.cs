using System.Text.Json;
using AutoMapper;
using MongoDB.Driver;
using PlaylistsService.Data;
using PlaylistsService.Dtos;
using PlaylistsService.Models;

namespace PlaylistsService.Events
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }

    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventProcessor(IMapper mapper, ILogger<EventProcessor> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _mapper = mapper;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = JsonSerializer.Deserialize<EventDto>(message);

            switch (eventType!.Event)
            {
                case EventType.SongUpdate:
                    await ProcessSongUpdate(message);
                    break;

                case EventType.SongDelete:
                    // 
                    break;

                default:
                    await Task.CompletedTask;
                    break;
            }
        }

        private async Task ProcessSongUpdate(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IMongoContext>();
                var song = context.Database.GetCollection<Song>("song");

                var songUpdateEventDto = JsonSerializer.Deserialize<SongUpdateEventDto>(message);

                try
                {
                    var songFromDb = await song.Find(s => s.ExternalId == songUpdateEventDto!.Id).FirstOrDefaultAsync();

                    songFromDb.Name = songUpdateEventDto!.Name;

                    await song.ReplaceOneAsync(s => s.Id == songFromDb.Id, songFromDb);

                    _logger.LogInformation($"Song with Id: {songFromDb.Id} updated");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Update song failed");
                }
            }
        }
    }

    public static class EventType
    {
        public const string SongUpdate = "Song_Updated";
        public const string SongDelete = "Song_Deleted";
    }
}