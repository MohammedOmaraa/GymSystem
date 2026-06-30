using GymSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;

        public MemberController(IMemberServices memberServices) {
            this.memberServices = memberServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Members = await memberServices.GetAllMembersAsync(ct);
            return View(Members);
        }
    }
}
