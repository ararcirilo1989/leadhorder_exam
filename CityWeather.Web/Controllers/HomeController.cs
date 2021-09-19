using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CityWeather.Web.Models;
using Microsoft.Extensions.Options;
using CityWeather.Web.Helpers;
using CityWeather.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace CityWeather.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IOptions<APISettingModel> apiSetting)
        {
            _logger = logger;
            APIConfig.apiUrl = apiSetting.Value.baseUrl;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UploadPartial()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<JsonResult> UploadPartial(IFormFile csvFile)
        {
            try
            {
                var file = Request.Form.Files[0];
                var filePath = Path.GetTempFileName();
                var fileExtension = Path.GetExtension(file.FileName);
                string[] sAllowedExtensions = { "csv" };

                if (Request.Form.Files.Count == 0)
                {
                    return Json(new { success = false, msg = "File not found." });
                }
                if (!sAllowedExtensions.Any(fileExtension.ToLower().Replace(".", "").Contains))
                {
                    return Json(new { success = false, msg = $"File is invalid. File must be csv." });
                }
                if (file.Length > (25 * 1024 * 1024))
                {
                    return Json(new { success = false, msg = "File size exceeds 25mb." });
                }
                
                var reader = new StreamReader(csvFile.OpenReadStream());

                var result = new StringBuilder();
                List<string> cities = new List<string>();

                while (reader.Peek() >= 0)
                {
                    string city = await reader.ReadLineAsync();
                    if (!city.Contains("City Name"))
                        cities.Add(city);
                }

                if(cities.Count() > 0)
                    return Json(new { success = true, cities });
                else
                    return Json(new { success = false, msg = "No City found in the file." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = ex.Message });
            }
            
        }

        public async Task<ActionResult> CitiesPartial(List<string> cityNames)
        {
            CityViewModel model = new CityViewModel();

            List<CityModel> emptyWeatherCities = new List<CityModel>();

            foreach (string cityName in cityNames)
            {
                emptyWeatherCities.Add(new CityModel { Name = cityName });
            }

            model.CityModels = await APIClientFactory.Instance.GetCityWeathers(emptyWeatherCities);

            return PartialView(model);
        }

    }
}
