using Equifax.CSV.Importer.Logic.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Equifax.CSV.Importer.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly ICSVService _csvService;
        private readonly IMemberService _memberService;

        public ImportController(ICSVService csvService,
            IMemberService memberService)
        {
            _csvService = csvService;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetMembersAsync();

            return View(members);
        }

        [HttpPost]
        public IActionResult Submit(IFormFile upload)
        {
            if (upload == null || upload.Length < 1)
            {
                ModelState.AddModelError("File", "Please Upload Your file");
                return View("Index");
            }

            if (!upload.FileName.EndsWith(".csv"))
            {
                ModelState.AddModelError("File", "This file format is not supported");
                return View("Index");
            }

            var conversion = _csvService.ReadFile(upload.OpenReadStream());

            if (!conversion.Success)
            {
                ModelState.AddModelError("File", conversion.Message);
                return View("Index");
            }

            var insert = _memberService.PersistMembers(conversion.Members);

            if (!insert.Success)
            {
                ModelState.AddModelError("File", insert.Message);
                return View("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
