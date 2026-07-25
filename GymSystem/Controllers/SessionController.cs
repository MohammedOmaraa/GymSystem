using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionsViewModels;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymSystem.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropDownsAsync(ct);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }
            var Result = await sessionServices.CreateSessionAsync(model, ct);

            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = Result.Message;

            await PopulateDropDownsAsync(ct);

            return View(model);
        }

        private async Task PopulateDropDownsAsync(CancellationToken ct)
        {
            var Categories = await sessionServices.GetCategoriesForDropDownAsync(ct);
            var Trainers = await sessionServices.GetTrainersForDropDownAsync(ct);
            ViewBag.Categories = new SelectList(Categories, "Id", "CategoryName");
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Session = await sessionServices.GetSessionByIdAsync(id, ct);
            if (Session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct) { 
            var result = await sessionServices.GetSessionToUpdateAsync(id, ct);
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropDownsAsync(ct);
            return View(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropDownsAsync(ct);
                return View(model);
            }
            var Result = await sessionServices.UpdateSessionAsync(id, model, ct);
            
            if (Result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Session updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            
            TempData["ErrorMessage"] = Result.Message;
            await PopulateDropDownsAsync(ct);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var session = await sessionServices.GetSessionByIdAsync(id, ct);
            if(session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }

            return View(session);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await sessionServices.RemoveSessionAsync(id, ct);
            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess ? "Session Deleted Successfullt" : Result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
