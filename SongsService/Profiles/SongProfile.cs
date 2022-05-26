using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using SongsService.Dtos;
using SongsService.Models;

namespace SongsService.Profiles
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<Song, SongReadDto>();

            CreateMap<SongCreateDto, Song>();

            // gRPC
            CreateMap<Song, SongGrpc>()
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => (DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)).ToTimestamp()));

            // Message queue
            CreateMap<Song, SongPublishDto>();
        }
    }
}