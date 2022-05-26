using ArtistsService;
using AutoMapper;
using Grpc.Net.Client;
using SongsService.Models;

namespace SongsService.Services
{
    public interface IGrpcArtistsClient
    {
        Artist? GetArtist(string id);
    }

    public class GrpcArtistsClient : IGrpcArtistsClient
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcArtistsClient> _logger;
        private readonly GrpcArtists.GrpcArtistsClient _client;

        public GrpcArtistsClient(IConfiguration configuration, IMapper mapper, ILogger<GrpcArtistsClient> logger)
        {
            _mapper = mapper;
            _logger = logger;

            // Get artistsServer url from appsetting
            string artistsServerUrl = configuration.GetValue<string>("Grpc:Artists");

            // Create connection to artistsServer
            var channel = GrpcChannel.ForAddress(artistsServerUrl);
            _client = new GrpcArtists.GrpcArtistsClient(channel);

            _logger.LogInformation($"Connected to ArtitstsServer with {artistsServerUrl}");
        }

        public Artist? GetArtist(string id)
        {
            var request = new GetArtistRequest { Id = id };

            try
            {
                var response = _client.GetArtist(request);
                return _mapper.Map<Artist>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail to get artist from ArtistsServer");
                return null;
            }
        }
    }
}