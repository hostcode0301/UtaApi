using AutoMapper;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using SongsService.Data;

namespace SongsService.Services
{
    public class GrpcSongsServer : GrpcSongs.GrpcSongsBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public GrpcSongsServer(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public override async Task<SongGrpc> GetSong(GetSongRequest request, ServerCallContext context)
        {
            var song = await _dbContext.Songs.FirstOrDefaultAsync(s => s.Id == request.Id);
            return _mapper.Map<SongGrpc>(song);
        }
    }
}