using System;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class VariatyController : Controller
    {
        private readonly DataContext _datacontext;

        public VariatyController(DataContext context)
        {
            _datacontext = context;
        }

        #region Index

        public IActionResult Index()
        {
            List<Variety> variaties = _datacontext.Varieties.ToList();
            return View(variaties);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Variety variaty)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _datacontext.Varieties.Add(variaty);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Variety variaty = _datacontext.Varieties.FirstOrDefault(x => x.Id == id);
            if (variaty == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(variaty);
        }

        [HttpPost]
        public IActionResult Edit(Variety variaty)
        {
            Variety existVariaty = _datacontext.Varieties.FirstOrDefault(x => x.Id == variaty.Id);
            if (existVariaty == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            existVariaty.Name = variaty.Name;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Variety variaty = _datacontext.Varieties.FirstOrDefault(x => x.Id == id);
            if (variaty == null) return Json(new { status = 404 });

            try
            {
                _datacontext.Varieties.Remove(variaty);
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