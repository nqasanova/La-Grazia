using System;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Controllers
{
    public class FaqController : Controller
    {
        private readonly DataContext _datacontext;

        public FaqController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        #region Index

        public IActionResult Index()
        {
            List<Faq> faqs = _datacontext.Faqs.ToList();

            return View(faqs);
        }

        #endregion
    }
}