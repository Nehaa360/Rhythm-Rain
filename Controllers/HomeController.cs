using Microsoft.AspNetCore.Mvc;
using MusicPlayer.Models;
using MusicPlayer.Services;
using System.Diagnostics;

namespace MusicPlayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherService _weatherService;
        private readonly SpotifyService _spotifyService;

        public HomeController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
                city = "Mumbai";  // Default city

            var weather = await _weatherService.GetWeatherAsync(city);
            return View(weather);
        }

        [HttpPost]
        public IActionResult MoodSelection(string mood)
        {
            return RedirectToAction("GetMoodPlaylists", "Music", new { mood });
        }

        public async Task<IActionResult> GetMoodWeatherPlaylists(string mood, string city)
        {
            var weather = await _weatherService.GetWeatherAsync(city);
            var playlists = await _spotifyService.GetPlaylistsBasedOnMoodAndWeatherAsync(mood, weather.Condition);
            return View(playlists);
        }

    }

}