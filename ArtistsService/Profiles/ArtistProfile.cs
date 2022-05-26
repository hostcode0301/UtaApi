using ArtistsService.Dtos;
using ArtistsService.Models;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace ArtistsService.Profiles
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<Artist, ArtistReadDto>();

            CreateMap<ArtistCreateDto, Artist>();

            CreateMap<Artist, ArtistGrpc>()
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.MapFrom(src => (DateTime.SpecifyKind(src.CreatedAt, DateTimeKind.Utc)).ToTimestamp()));
        }
    }
}