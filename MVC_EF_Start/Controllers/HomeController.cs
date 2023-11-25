using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.Models;
using MVC_EF_Start.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            //httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string WA_EV_API_PATH = BASE_URL;
            string electricVehicleData = "";

            //Vehicles? vehicles = null;

            List<Vehicle> waVehiclesList = null;

            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(WA_EV_API_PATH);


            try
            {
                HttpResponseMessage response = httpClient.GetAsync(WA_EV_API_PATH)
                                                        .GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    electricVehicleData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!electricVehicleData.Equals(""))
                {
                    // Deserialize the entire list
                    List<Vehicle> vehiclesList = JsonConvert.DeserializeObject<List<Vehicle>>(electricVehicleData);

                    // Filter the list to include only rows where the State is WA
                    waVehiclesList = vehiclesList.Where(v => v.State == "WA").ToList();

                    // Filter out duplicates based on VinNumber
                    waVehiclesList = waVehiclesList.GroupBy(v => v.VinNumber).Select(g => g.First()).ToList();

                    foreach (var waVehicle in waVehiclesList)
                    {
                        var existingVehicle = dbContext.Vehicles.Find(waVehicle.VinNumber);

                        if (existingVehicle != null)
                        {
                            // Detach existing entity
                            dbContext.Entry(existingVehicle).State = EntityState.Detached;
                        }

                        // Add new entity
                        dbContext.Vehicles.Add(waVehicle);
                    }

                    await dbContext.SaveChangesAsync();
                }

                try
                {
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving changes: {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                // Log or print the outer exception
                Console.WriteLine($"Outer Exception: {ex.Message}");

                // Check if there's an inner exception
                if (ex.InnerException != null)
                {
                    // Log or print the inner exception
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                // Handle the exception as needed
            }

            return View(waVehiclesList);
            //return View();

        }

        // connects to Home Page
        public IActionResult EVHomePage()
        {
            return View();
        }

        // connects to main page with chart
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