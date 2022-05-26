using AutoMapper;
using Grpc.Net.Client;
using PlaylistsService.Models;
using SongsService;

namespace PlaylistsService.Services
{
    public interface IGrpcSongsClient
    {
        Song? GetSong(int id);
    }

    public class GrpcSongsClient : IGrpcSongsClient
    {
        private readonly GrpcSongs.GrpcSongsClient _client;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcSongsClient> _logger;

        public GrpcSongsClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcSongsClient> logger)
        {
            string songsServiceUrl = configuration.GetValue<string>("Grpc:Songs");

            var channel = GrpcChannel.ForAddress(songsServiceUrl);
            _client = new GrpcSongs.GrpcSongsClient(channel);
            logger.LogInformation($"GrpcSongClient created with url {songsServiceUrl}");

            _mapper = mapper;
            _logger = logger;
        }

        public Song? GetSong(int id)
        {
            var request = new GetSongRequest { Id = id };

            try
            {
                var response = _client.GetSong(request);
                return _mapper.Map<Song>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting song with id {id}");
                return null;
            }
        }
    }
}