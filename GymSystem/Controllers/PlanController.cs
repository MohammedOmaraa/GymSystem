using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGrnericRepository<Plan> planRepository;
        public PlanController(IGrnericRepository<Plan> planRepository)
        {
            this.planRepository = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepository.GetAllAsync(false, token);
            
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken token)
        {
            var plan = await planRepository.GetByIdAsync(id, token);
            
            if (plan == null)
                return NotFound();

            return View(plan);
        }
    }
}
