using ArtistsService.Data;
using ArtistsService.Dtos;
using ArtistsService.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ArtistsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IMongoCollection<Artist> _artists;
        private readonly IMapper _mapper;

        public ArtistsController(IMongoContext context, IMapper mapper)
        {
            _artists = context.Database.GetCollection<Artist>("artist");
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistReadDto>>> Get()
        {
            var artistsFromModel = await _artists.Find(a => true).ToListAsync();
            var artistsDto = _mapper.Map<IEnumerable<ArtistReadDto>>(artistsFromModel);

            return Ok(artistsDto);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ArtistCreateDto artistCreateDto)
        {
            var newArtist = _mapper.Map<Artist>(artistCreateDto);
            await _artists.InsertOneAsync(newArtist);

            return Ok();
        }
    }
}