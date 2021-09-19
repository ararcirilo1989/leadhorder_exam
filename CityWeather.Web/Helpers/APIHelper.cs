using CityWeather.Web.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CityWeather.Web.Helpers
{
    public class APIHelper
    {
        private readonly HttpClient hc;

        private Uri BaseEndpoint { get; set; }

        public APIHelper(Uri apiUri)
        {
            if (apiUri == null)
            {
                throw new ArgumentNullException("apiUri");
            }
            BaseEndpoint = apiUri;
            hc = new HttpClient();
        }

        #region Utils

        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        #endregion

        public async Task<IEnumerable<CityModel>> GetCityWeathers(List<CityModel> cities)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                $"weather"));

            var response = await hc.PostAsync(requestUrl.ToString(), CreateHttpContent<IEnumerable<CityModel>>(cities));

            var data = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<CityModel>>(data);
        }
        

    }
}
