using GymSystem.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanRepository planRepository;
        public PlanController()
        {
            planRepository = new PlanRepository();
        }
        public async Task<IActionResult> Index()
        {
            var plans = await planRepository.GetAllPlansAsync();
            
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepository.GetPlanByIdAsync(id);
            
            if (plan == null)
                return NotFound();

            return View(plan);
        }
    }
}
