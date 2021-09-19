using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeather.Web.Models
{
    public class APISettingModel
    {
        public string baseUrl { get; set; }
    }

    public class APIConfig
    {
        public static string apiUrl { get; set; }
    }
}
