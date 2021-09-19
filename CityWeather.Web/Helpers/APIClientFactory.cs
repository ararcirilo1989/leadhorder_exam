using CityWeather.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CityWeather.Web.Helpers
{
    internal static class APIClientFactory
    {
        private static Uri apiUrl;
        private static Lazy<APIHelper> apiHelper = new Lazy<APIHelper>(() => new APIHelper(apiUrl), LazyThreadSafetyMode.ExecutionAndPublication);

        static APIClientFactory()
        {
            apiUrl = new Uri(APIConfig.apiUrl);
        }

        public static APIHelper Instance
        {
            get
            {
                return apiHelper.Value;
            }
        }
    }
}
