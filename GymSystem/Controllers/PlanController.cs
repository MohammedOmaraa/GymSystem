using GymSystem.DAL.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly GymDbContext _dbContext = new GymDbContext();
        
        public async Task<IActionResult> Index()
        {
            var plans = await _dbContext.Plans.ToListAsync();
            
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
            
            if (plan == null)
                return NotFound();

            return View(plan);
        }
    }
}
