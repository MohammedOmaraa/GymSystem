using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices memberServices;
        private readonly IAttachmentServices attachmentServices;

        public MemberController(IMemberServices memberServices, IAttachmentServices attachmentServices) {
            this.memberServices = memberServices;
            this.attachmentServices = attachmentServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var tempResults = TempData["Results"];
            var Members = await memberServices.GetAllMembersAsync(ct);
            return View(Members);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await memberServices.CreateMemberAsync(model, ct);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Message;

                return View(nameof(Create), model);
            }

            TempData["Success"] = "Member created successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var member = await memberServices.GetMemberDetailsAsync(id, ct);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthRecord = await memberServices.GetMemberHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }

        [HttpGet]
        public IActionResult Photo(string fileName)
        {
            var result = attachmentServices.GetFile(fileName, "MemberPictures");

            if (!result.IsSuccess)
                return NotFound();

            return File(result.Value!.Stream, result.Value.ContentType);
        }

    }
}
