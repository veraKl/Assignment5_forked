using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.Models;
using MVC_EF_Start.DataAccess;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_EF_Start.Controllers
{
  public class HomeController : Controller
  {

        HttpClient? httpClient;

        static string BASE_URL = "https://data.wa.gov/resource/f6w7-q2d2.json";
        static string API_KEY = "hcEy0sdQxZG53NCsiQQIsBGH8tmwt2o3gbPmmwcG";

        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string WA_EV_API_PATH = BASE_URL;
            string electricVehicleData = "";

            Vehicles? vehicles = null;

            //httpClient.BaseAddress = new Uri(NATIONAL_PARK_API_PATH);
            httpClient.BaseAddress = new Uri(WA_EV_API_PATH);

            try
            {
                //HttpResponseMessage response = httpClient.GetAsync(NATIONAL_PARK_API_PATH)
                //                                        .GetAwaiter().GetResult();
                HttpResponseMessage response = httpClient.GetAsync(WA_EV_API_PATH)
                                                        .GetAwaiter().GetResult();



                if (response.IsSuccessStatusCode)
                {
                    electricVehicleData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!electricVehicleData.Equals(""))
                {
                    //JsonConvert is part of the NewtonSoft.Json Nuget package
                    vehicles = JsonConvert.DeserializeObject<Vehicles>(electricVehicleData);
                }

                dbContext.Vehicles.Add(vehicles);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return View(vehicles);
            //return View();
        }
    }
}