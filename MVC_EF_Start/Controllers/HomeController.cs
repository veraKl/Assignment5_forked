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
    public IActionResult Index()
    {
      return View();
    }
  }
}