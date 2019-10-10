using Equifax.CSV.Importer.Logic.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Equifax.CSV.Importer.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly ICSVService _csvService;

        public ImportController(ICSVService csvService)
        {
            _csvService = csvService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ModelState.AddModelError("File", "There was a problem converting your file");
                return View("Index");
            }

            var insert = _csvService.PersistMembers(conversion);

            if (!insert.Success)
            {
                ModelState.AddModelError("File", "There was a problem persisting the members to the database");
                return View("Index");
            }

            return View("Index");
        }
    }
}
