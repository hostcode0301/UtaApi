using ArtistsService.Data;
using ArtistsService.Models;
using AutoMapper;
using Grpc.Core;
using MongoDB.Driver;

namespace ArtistsService.Services
{
    public class GrpcArtistsServer : GrpcArtists.GrpcArtistsBase
    {
        private readonly IMongoCollection<Artist> _artists;
        private readonly IMapper _mapper;

        public GrpcArtistsServer(IMongoContext context, IMapper mapper)
        {
            _artists = context.Database.GetCollection<Artist>("artist");
            _mapper = mapper;
        }

        public override async Task<ArtistGrpc> GetArtist(GetArtistRequest request, ServerCallContext context)
        {
            var artistFromModel = await _artists.Find(a => a.Id == request.Id).FirstOrDefaultAsync();

            var artistDto = _mapper.Map<ArtistGrpc>(artistFromModel);

            return artistDto;
        }
    }
}