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

    public class AboutController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _env;

        public AboutController(DataContext datacontext, IWebHostEnvironment env)
        {
            _datacontext = datacontext;
            _env = env;
        }

        #region Index

        public IActionResult Index()
        {
            About about = _datacontext.Abouts.FirstOrDefault();
            return View(about);
        }

        #endregion

        #region Edit

        public IActionResult Edit()
        {
            About about = _datacontext.Abouts.FirstOrDefault();
            return View(about);
        }

        [HttpPost]
        public IActionResult Edit(About about)
        {

            About existAbout = _datacontext.Abouts.FirstOrDefault();

            if (existAbout == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            if (!ModelState.IsValid)
            {
                return View(existAbout);
            }

            if (about.ImageFile != null)
            {
                if (about.ImageFile.ContentType != "image/png" && about.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type can only be jpeg,jpg or png!");
                    return View(existAbout);
                }

                if (about.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2MB!");
                    return View(existAbout);
                }

                if (existAbout.Image != null)
                {
                    FileValidator.Delete(_env.WebRootPath, "uploads/about", existAbout.Image);
                }

                string aboutFileName = FileValidator.Save(_env.WebRootPath, "uploads/about", about.ImageFile);

                existAbout.Image = aboutFileName;
            }

            else
            {
                if (about.Image == null)
                {
                    if (existAbout.Image != null)
                    {
                        FileValidator.Delete(_env.WebRootPath, "uploads/about", existAbout.Image);
                        existAbout.Image = null;
                    }
                }
            }

            existAbout.Title = about.Title;
            existAbout.Description = about.Description;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }
    }

    #endregion
}