using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SOPasswordManager.Filters;
using SOPasswordManager.Models;

namespace SOPasswordManager.Controllers
{
    [AuthoriseFilter]
    public class DashboadController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Test= HttpContext.Session.GetString("MemberSession");

            return View();
        }

        public IActionResult About(string abc)
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
