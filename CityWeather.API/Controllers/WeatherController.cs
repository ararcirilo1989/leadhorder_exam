using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CityWeather.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CityWeather.API.Controllers
{

    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        
        [HttpPost]
        public async Task<IEnumerable<CityViewModel>> GetCityWeathers(IEnumerable<CityViewModel> cities)
        {
            List<CityViewModel> newCities = new List<CityViewModel>();
            foreach (CityViewModel city in cities)
            {
                CityViewModel newCity = await GetCityWeather(city.Name);
                newCities.Add(newCity);
            }
            return newCities;
        }

        #region Utils

        private async Task<CityViewModel> GetCityWeather(string cityName)
        {
            CityViewModel newCity = new CityViewModel();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = CreateRequestUri($"https://community-open-weather-map.p.rapidapi.com/weather", $"units=metric&q={cityName}"),
                    Headers =
                {
                    { "x-rapidapi-host", "community-open-weather-map.p.rapidapi.com" },
                    { "x-rapidapi-key", "69980d5298msh4eb076bdb91ce6dp135aa2jsn5e4bea98726a" },
                },
                };
                var response = await client.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                newCity = JsonConvert.DeserializeObject<CityViewModel>(body);

                return newCity;
            }
            catch (Exception ex)
            {
                return new CityViewModel { Name = cityName, Error = "Error loading the weather of this City." };
            }
            
        }

        private Uri CreateRequestUri(string uri, string q = "")
        {
            var endpoint = new Uri(uri);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = q;
            return uriBuilder.Uri;
        }

        #endregion
    }
}
