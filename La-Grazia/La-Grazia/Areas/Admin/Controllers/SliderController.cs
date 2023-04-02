using System;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;
using La_Grazia.Validators;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _environment;

        public SliderController(DataContext datacontext, IWebHostEnvironment environment)
        {
            _datacontext = datacontext;
            _environment = environment;
        }

        #region Index

        public IActionResult Index()
        {
            List<Slider> sliders = _datacontext.Sliders.ToList();
            return View(sliders);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type can only be jpeg or png!");
                    return View();
                }

                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2mb!");
                    return View();
                }

                string fileName = FileValidator.Save(_environment.WebRootPath, "uploads/slider", slider.ImageFile);

                slider.Image = fileName;
            }

            if (slider.Order < 0)
            {
                ModelState.AddModelError("Order", "Order cannot be less than 0!");
                return View();
            }

            _datacontext.Sliders.Add(slider);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Slider slider = _datacontext.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            Slider existslider = _datacontext.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existslider == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            string fileName = null;

            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/png" && slider.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type can only be jpeg,jpg or png!");
                    return View(existslider);
                }

                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2MB!");
                    return View(existslider);
                }

                fileName = FileValidator.Save(_environment.WebRootPath, "uploads/slider", slider.ImageFile);
            }

            if (fileName != null || slider.Image == null)
            {
                if (existslider.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/slider", existslider.Image);
                }

                existslider.Image = fileName;
            }

            if (slider.Order < 0)
            {
                ModelState.AddModelError("Order", "Order cannot be less than 0!");
                return View();
            }

            existslider.Order = slider.Order;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Slider slider = _datacontext.Sliders.FirstOrDefault(x => x.Id == id);
            if (slider == null) return Json(new { status = 404 });

            try
            {
                if (slider.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/slider", slider.Image);
                }

                _datacontext.Sliders.Remove(slider);
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