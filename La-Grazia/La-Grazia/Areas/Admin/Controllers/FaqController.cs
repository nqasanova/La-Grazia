using System;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class FaqController : Controller
    {
        private readonly DataContext _datacontext;

        public FaqController(DataContext context)
        {
            _datacontext = context;
        }

        #region Index

        public IActionResult Index()
        {
            List<Faq> faqs = _datacontext.Faqs.ToList();
            return View(faqs);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (faq.Order < 0)
            {
                ModelState.AddModelError("Order", "Order cannot be less than 0");
                return View();
            }

            _datacontext.Faqs.Add(faq);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Faq faq = _datacontext.Faqs.FirstOrDefault(x => x.Id == id);
            if (faq == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(faq);
        }

        [HttpPost]
        public IActionResult Edit(Faq faq)
        {
            Faq existFaq = _datacontext.Faqs.FirstOrDefault(x => x.Id == faq.Id);
            if (existFaq == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            if (faq.Order < 0)
            {
                ModelState.AddModelError("Order", "Order can not be less than 0");
                return View();
            }

            existFaq.Order = faq.Order;
            existFaq.Question = faq.Question;
            existFaq.Answer = faq.Answer;

            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Faq faq = _datacontext.Faqs.FirstOrDefault(x => x.Id == id);
            if (faq == null) return Json(new { status = 404 });

            try
            {
                _datacontext.Faqs.Remove(faq);
                _datacontext.SaveChanges();
            }

            catch (Exception)
            {
                return Json(new { status = 500 });
            }

            return Json(new { status = 200 });
        }

        #endregion
    }
}