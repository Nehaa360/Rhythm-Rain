using MusicPlayer.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MusicPlayer.Services
{
    public class WeatherService
    {
        private readonly string _apiKey = "abcaac70f7c8ddd48b845fa9c96f00f7"; // Your OpenWeatherMap API key
        private readonly string _apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}";

        public async Task<WeatherInfo> GetWeatherAsync(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var url = string.Format(_apiUrl, city, _apiKey);
                    var response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        // Read the response content for debugging
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Failed to fetch weather data. Status Code: {response.StatusCode}, Content: {errorContent}");
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    dynamic weatherData = JsonConvert.DeserializeObject(content);

                    return new WeatherInfo
                    {
                        City = weatherData.name,
                        Condition = weatherData.weather[0].description,
                        Temperature = weatherData.main.temp - 273.15 // Kelvin to Celsius
                    };
                }
                catch (HttpRequestException e)
                {
                    throw new Exception("Error connecting to the weather service: " + e.Message);
                }
                catch (JsonException e)
                {
                    throw new Exception("Error parsing weather data: " + e.Message);
                }
            }
        }
    }
}
