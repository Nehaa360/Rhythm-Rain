using MusicPlayer.Models;
using SpotifyAPI.Web;

namespace MusicPlayer.Services
{
    public class SpotifyService
    {
        private readonly SpotifyClient _spotify;

        public SpotifyService()
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest("5af981d350784b5296422d8768803817", "c75d20156cc6402cab7283c50b5636dc");
            var response = new OAuthClient(config).RequestToken(request).Result;

            _spotify = new SpotifyClient(config.WithToken(response.AccessToken));
        }

        public async Task<List<Playlist>> GetPlaylistsBasedOnMoodAsync(string mood)
        {
            var searchResults = await _spotify.Search.Item(new SearchRequest(SearchRequest.Types.Playlist, mood));
            return searchResults.Playlists.Items.Select(p => new Playlist
            {
                PlaylistId = p.Id,
                Name = p.Name,
                ImageUrl = p.Images.FirstOrDefault()?.Url,
                PlaylistUrl = p.ExternalUrls["spotify"]
            }).ToList();
        }

        public async Task<List<Playlist>> GetPlaylistsBasedOnMoodAndWeatherAsync(string mood, string weatherCondition)
        {
            string query = $"{mood} {weatherCondition}";
            var searchResults = await _spotify.Search.Item(new SearchRequest(SearchRequest.Types.Playlist, query));

            return searchResults.Playlists.Items.Select(p => new Playlist
            {
                PlaylistId = p.Id,
                Name = p.Name,
                ImageUrl = p.Images.FirstOrDefault()?.Url,
                PlaylistUrl = p.ExternalUrls["spotify"]
            }).ToList();
        }

    }

}