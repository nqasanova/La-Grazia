using System;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Admin.Controllers
{
    public class ErrorController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}