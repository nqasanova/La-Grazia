using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TypeController : Controller
    {
        private readonly DataContext _datacontext;

        public TypeController(DataContext context)
        {
            _datacontext = context;
        }

        #region Index

        public IActionResult Index()
        {
            List<Database.Models.Type> types = _datacontext.Types.ToList();
            return View(types);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Database.Models.Type type)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            _datacontext.Types.Add(type);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Database.Models.Type type = _datacontext.Types.FirstOrDefault(x => x.Id == id);
            if (type == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(type);
        }

        [HttpPost]
        public IActionResult Edit(Database.Models.Type type)
        {
            Database.Models.Type existType = _datacontext.Types.FirstOrDefault(x => x.Id == type.Id);
            if (existType == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            existType.Name = type.Name;

            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Database.Models.Type type = _datacontext.Types.FirstOrDefault(x => x.Id == id);
            if (type == null) return Json(new { status = 404 });

            try
            {
                _datacontext.Types.Remove(type);
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