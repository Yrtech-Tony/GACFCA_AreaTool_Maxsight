﻿using Microsoft.AspNetCore.Mvc;
using FIAT.Web.Common;

namespace FIAT.Web.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        [PermissionRequired]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
