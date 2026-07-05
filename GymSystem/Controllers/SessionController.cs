using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionServices sessionServices;

        public SessionController(ISessionServices sessionServices)
        {
            this.sessionServices = sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Sessions = await sessionServices.GetAllSessionsAsync(ct);
            return View(Sessions);
        }
    }
}
