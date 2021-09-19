using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityWeather.Web.ViewModels
{

    public class CityViewModel
    {
        public string Name { get; set; }
        public Main Main { get; set; }
        public IEnumerable<Weather> Weather { get; set; }
        public Wind Wind { get; set; }
        public long Dt{ get; set; }
        public string Error { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public double Umidity { get; set; }
    }

    public class Weather
    {
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public int Degree { get; set; }
    }
}

