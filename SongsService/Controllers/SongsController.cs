using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsService.Data;
using SongsService.Dtos;
using SongsService.Models;
using SongsService.Services;

namespace SongsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IGrpcArtistsClient _artistsClient;
        private readonly IMessageBusPublisher _publisher;
        private readonly ILogger<SongsController> _logger;

        public SongsController(
            AppDbContext context,
            IMapper mapper,
            IGrpcArtistsClient artistsClient,
            IMessageBusPublisher publisher,
            ILogger<SongsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _artistsClient = artistsClient;
            _publisher = publisher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongReadDto>>> Get()
        {
            var songs = await _context.Songs.ToListAsync();
            var songsDto = _mapper.Map<IEnumerable<SongReadDto>>(songs);
            return Ok(songsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SongReadDto>> GetById(int id)
        {
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);
            if (song is null)
            {
                return NotFound();
            }

            // Get artists From ArtistsService
            foreach (var artistId in song.ArtistId)
            {
                var artist = _artistsClient.GetArtist(artistId);

                if (song.Artists is null)
                {
                    song.Artists = new List<Artist>();
                }

                song.Artists.Add(artist!);
            }

            var songDto = _mapper.Map<SongReadDto>(song);
            return Ok(songDto);
        }

        [HttpPost]
        public async Task<ActionResult<SongReadDto>> Post(SongCreateDto songDto)
        {
            var song = _mapper.Map<Song>(songDto);

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, SongUpdateDto songUpdateDto)
        {
            // Handle update song
            var song = await _context.Songs.FirstOrDefaultAsync(s => s.Id == id);

            if (song is null)
            {
                return NotFound();
            }

            song.Name = songUpdateDto.Name!;
            var rs = await _context.SaveChangesAsync() > 0;

            // Handle publish message
            try
            {
                var songPublishDto = _mapper.Map<SongPublishDto>(song);
                songPublishDto.Event = "Song_Updated";

                _publisher.PublishChangeSong(songPublishDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message");
            }

            return Ok();
        }
    }
}