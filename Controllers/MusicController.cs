using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Services;

namespace MusicPlayer.Controllers
{
    public class MusicController : Controller
    {
        private readonly SpotifyService _spotifyService;

        public MusicController(SpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }

        public async Task<IActionResult> GetMoodPlaylists(string mood)
        {
            var playlists = await _spotifyService.GetPlaylistsBasedOnMoodAsync(mood);
            return View(playlists);
        }
    }

}