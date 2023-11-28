using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.Models;
using MVC_EF_Start.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.IO;
using static MVC_EF_Start.Models.EFModels2;

namespace MVC_EF_Start.Controllers
{
    public class HomeController : Controller
    {
        HttpClient? httpClient;

        static string BASE_URL = "https://data.wa.gov/resource/f6w7-q2d2.json?$query=SELECT%20vin_1_10%2C%20county%2C%20city%2C%20state%2C%20zip_code%2C%20model_year%2C%20make%2C%20model%2C%20ev_type%2C%20cafv_type%2C%20electric_range%2C%20base_msrp%2C%20legislative_district%2C%20dol_vehicle_id%2C%20geocoded_column%2C%20electric_utility%2C%20_2020_census_tract%20ORDER%20BY%20%3Aid%20ASC";

        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string WA_EV_API_PATH = BASE_URL;
            string electricVehicleData = "";

            List<Vehicle> waVehiclesList = null;

            httpClient.BaseAddress = new Uri(WA_EV_API_PATH);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(WA_EV_API_PATH);

                if (response.IsSuccessStatusCode)
                {
                    electricVehicleData = await response.Content.ReadAsStringAsync();
                }

                if (!electricVehicleData.Equals(""))
                {
                    List<Vehicle> vehiclesList = JsonConvert.DeserializeObject<List<Vehicle>>(electricVehicleData);

                    waVehiclesList = vehiclesList
                        .Where(v => v.State == "WA")
                        .GroupBy(v => v.VinNumber)
                        .Select(g => g.First())
                        .ToList();

                    foreach (var waVehicle in waVehiclesList)
                    {
                        var existingVehicle = dbContext.Vehicles.Find(waVehicle.VinNumber);

                        if (existingVehicle != null)
                        {
                            dbContext.Entry(existingVehicle).State = EntityState.Detached;
                        }

                        dbContext.Vehicles.Add(waVehicle);
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Outer Exception: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }

            return View(waVehiclesList);
        }

        public IActionResult EVHomePage()
        {
            return View();
        }
        /*
        public IActionResult RegionGrouping()
        {
            var countyRegionMapping = new Dictionary<string, string>
            {
                { "Stevens", "Northeast" },
                { "Spokane", "Northeast" },
                { "Skagit", "Northwest" },
                { "Snohomish", "Northwest" },
                { "Island", "Northwest" },
                { "Chelan", "Southeast" },
                { "Grant", "Southeast" },
                { "Whitman", "Southeast" },
                { "Yakima", "Southeast" },
                { "Klickitat", "Southeast" },
                { "Walla Walla", "Southeast" },
                { "King", "South Puget Sound" },
                { "Thurston", "South Puget Sound" },
                { "Kitsap", "South Puget Sound" },
                { "Clallam", "Olympic" },
                { "Jefferson", "Olympic" },
                { "Cowlitz", "Pacific Cascade" },
                { "Clark", "Pacific Cascade" }
            };

            var countyNames = dbContext.Counties.Select(c => c.CountyName).ToList();

            var vehiclesForCounties = dbContext.Vehicles
                .Include(v => v.CountyNavigation)
                .Where(v => countyNames.Contains(v.CountyNavigation.CountyName))
                .ToList();

            var regionData = vehiclesForCounties
                .GroupBy(v => v.CountyNavigation.CountyName)
                .Select(g => new RegionViewModel
                {
                    RegionName = countyRegionMapping.GetValueOrDefault(g.Key, "Unknown"),
                    TotalCars = g.Count()
                })
                .GroupBy(r => r.RegionName)
                .Select(g => new RegionViewModel
                {
                    RegionName = g.Key,
                    TotalCars = g.Sum(r => r.TotalCars)
                })
                .ToList();

            // Convert the complex object to a Dictionary
            var serializableRegionData = regionData.ToDictionary(r => r.RegionName, r => (object)r.TotalCars);

           // Store the region data in Session instead of TempData
    HttpContext.Session.Set("RegionData", serializableRegionData);

            return RedirectToAction("MainPage");
        }
        */

        public IActionResult MainPage()
        {
            return View();
          
        }
        // connects to detail page with tables and CRUD functions
        public IActionResult DetailPage()
        {
            return View();
        }


        // connects to the about us page
        public IActionResult AboutUsPage()
        {
            return View();
        }

    }
}