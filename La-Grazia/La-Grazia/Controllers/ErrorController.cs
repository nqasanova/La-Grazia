using System;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}