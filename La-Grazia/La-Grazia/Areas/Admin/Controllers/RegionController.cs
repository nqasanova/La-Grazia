//using System;
//using La_Grazia.Database.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using La_Grazia.Database;

//namespace La_Grazia.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = "Admin,SuperAdmin")]
//    public class RegionController : Controller
//    {
//        private readonly DataContext _datacontext;

//        public RegionController(DataContext context)
//        {
//            _datacontext = context;
//        }

//        #region Index

//        public IActionResult Index()
//        {
//            List<Region> regions = _datacontext.Regions.ToList();
//            return View(regions);
//        }

//        #endregion

//        #region Create

//        public IActionResult Create()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Create(Region region)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View();
//            }

//            _datacontext.Regions.Add(region);
//            _datacontext.SaveChanges();

//            return RedirectToAction("index");
//        }

//        #endregion

//        #region Edit

//        public IActionResult Edit(int id)
//        {
//            Region region = _datacontext.Regions.FirstOrDefault(x => x.Id == id);
//            if (region == null)
//            {
//                return RedirectToAction("index", "error", new { area = "" });
//            }

//            return View(region);
//        }

//        [HttpPost]
//        public IActionResult Edit(Region region)
//        {
//            Region existRegion = _datacontext.Regions.FirstOrDefault(x => x.Id == region.Id);
//            if (existRegion == null)
//            {
//                return RedirectToAction("index", "error", new { area = "" });
//            }

//            existRegion.Name = region.Name;

//            _datacontext.SaveChanges();

//            return RedirectToAction("index");
//        }

//        #endregion

//        #region Delete

//        public IActionResult DeleteFetch(int id)
//        {
//            Region region = _datacontext.Regions.FirstOrDefault(x => x.Id == id);
//            if (region == null) return Json(new { status = 404 });

//            try
//            {
//                _datacontext.Regions.Remove(region);
//                _datacontext.SaveChanges();
//            }

//            catch (Exception)
//            {
//                return Json(new { status = 500 });
//            }

//            return Json(new { status = 200 });
//        }

//        #endregion
//    }
//}