using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using PlaylistsService.Data;
using PlaylistsService.Dtos;
using PlaylistsService.Models;
using PlaylistsService.Services;

namespace PlaylistsService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaylistsController : Controller
    {
        private readonly MongoDB.Driver.IMongoCollection<Playlist> _playlists;
        private readonly MongoDB.Driver.IMongoCollection<Song> _songs;
        private readonly IMapper _mapper;
        private readonly IGrpcSongsClient _songClient;

        public PlaylistsController(IMongoContext context, IMapper mapper, IGrpcSongsClient songClient)
        {
            _playlists = context.Database.GetCollection<Playlist>("playlist");
            _songs = context.Database.GetCollection<Song>("song");
            _mapper = mapper;
            _songClient = songClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<PlaylistReadDto>>> Get()
        {
            var lookup = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument
                    {
                        { "from", "song" },
                        { "localField", "SongIds" },
                        { "foreignField", "_id" },
                        { "as", "Songs" }
                    }
                }
            };

            var playlists = await _playlists.Aggregate().AppendStage<Playlist>(lookup).ToListAsync();
            return _mapper.Map<List<PlaylistReadDto>>(playlists);
        }

        [HttpGet("{id:length(24)}", Name = "GetPlaylist")]
        public async Task<ActionResult<PlaylistReadDto>> Get(string id)
        {
            var playlist = await _playlists.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound();
            }

            return _mapper.Map<PlaylistReadDto>(playlist);
        }

        [HttpPost]
        public async Task<ActionResult<PlaylistReadDto>> Create(PlaylistCreateDto playlistCreateDto)
        {
            var playlist = _mapper.Map<Playlist>(playlistCreateDto);
            await _playlists.InsertOneAsync(playlist);

            var playlistReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return CreatedAtRoute("GetPlaylist", new { id = playlistReadDto.Id }, playlistReadDto);
        }

        [HttpPost("{id:length(24)}/songs")]
        public async Task<ActionResult<PlaylistReadDto>> AddSong(string id, SongCreateDto songCreateDto)
        {
            var playlist = await _playlists.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (playlist == null)
            {
                return NotFound();
            }

            foreach (var songId in songCreateDto.ExternalIds)
            {
                var song = _songClient.GetSong(songId);

                if (song != null)
                {
                    await _songs.InsertOneAsync(song);

                    if (playlist.SongIds == null)
                    {
                        playlist.SongIds = new List<ObjectId>();
                    }

                    playlist.SongIds.Add(ObjectId.Parse(song.Id));
                }
            }

            await _playlists.ReplaceOneAsync(p => p.Id == playlist.Id, playlist);

            var playlistReadDto = _mapper.Map<PlaylistReadDto>(playlist);
            return CreatedAtRoute("GetPlaylist", new { id = playlistReadDto.Id }, playlistReadDto);
        }
    }
}