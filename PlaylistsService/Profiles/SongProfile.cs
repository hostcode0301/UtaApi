using AutoMapper;
using PlaylistsService.Models;
using PlaylistsService.Dtos;
using SongsService;

namespace PlaylistsService.Profiles
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<SongGrpc, Song>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()));

            CreateMap<Song, SongReadDto>();
        }
    }
}