using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Equifax.CSV.Importer.Web.Models;

namespace Equifax.CSV.Importer.Web.Controllers
{
    public class ImportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
