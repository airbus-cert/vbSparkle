using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vbSparkle.Web.Models;

namespace vbSparkle.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public ActionResult Deobfuscate(CodeUploadModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                model.After = VbPartialEvaluator.PrettifyEncoded(model.Before);
                               

            }

            return View("Index", model);
        }
    }
}
