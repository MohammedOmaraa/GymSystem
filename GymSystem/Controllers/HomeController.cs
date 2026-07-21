using GymSystem.BLL.Services.Interfaces;
using GymSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticsServices analyticsServices;

        public HomeController(IAnalyticsServices analyticsServices)
        {
            this.analyticsServices = analyticsServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var Data = await analyticsServices.GetAnalyticsDataAsync(ct);
            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
