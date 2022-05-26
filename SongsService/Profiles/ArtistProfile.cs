using ArtistsService;
using AutoMapper;
using SongsService.Dtos;
using SongsService.Models;

namespace SongsService.Profiles
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<ArtistGrpc, Artist>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToDateTime()));

            CreateMap<Artist, ArtistReadDto>();
        }
    }
}