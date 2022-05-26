using AutoMapper;
using PlaylistsService.Dtos;
using PlaylistsService.Models;

namespace PlaylistsService.Profiles
{
    public class PlaylistProfile : Profile
    {
        public PlaylistProfile()
        {
            CreateMap<Playlist, PlaylistReadDto>();

            CreateMap<PlaylistCreateDto, Playlist>();
        }
    }
}