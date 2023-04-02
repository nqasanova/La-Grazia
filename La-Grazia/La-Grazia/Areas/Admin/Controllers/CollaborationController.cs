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
    public class CollaborationController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _environment;

        public CollaborationController(DataContext datacontext, IWebHostEnvironment environment)
        {
            _datacontext = datacontext;
            _environment = environment;
        }

        #region Index

        public IActionResult Index()
        {
            List<Collaboration> Collaborations = _datacontext.Collaborations.ToList();
            return View(Collaborations);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Collaboration Collaboration)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (Collaboration.ImageFile != null)
            {
                if (Collaboration.ImageFile.ContentType != "image/jpeg" && Collaboration.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type can only be jpeg or png!");
                    return View();
                }

                if (Collaboration.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2mb!");
                    return View();
                }

                string fileName = FileValidator.Save(_environment.WebRootPath, "uploads/Collaboration", Collaboration.ImageFile);

                Collaboration.Image = fileName;
            }

            _datacontext.Collaborations.Add(Collaboration);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Collaboration Collaboration = _datacontext.Collaborations.FirstOrDefault(x => x.Id == id);

            if (Collaboration == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(Collaboration);
        }

        [HttpPost]
        public IActionResult Edit(Collaboration Collaboration)
        {
            if (!ModelState.IsValid) return View();

            Collaboration existCollaboration = _datacontext.Collaborations.FirstOrDefault(x => x.Id == Collaboration.Id);

            if (existCollaboration == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            string fileName = null;

            if (Collaboration.ImageFile != null)
            {
                if (Collaboration.ImageFile.ContentType != "image/png" && Collaboration.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type can only be jpeg,jpg or png!");
                    return View(existCollaboration);
                }

                if (Collaboration.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2MB!");
                    return View(existCollaboration);
                }

                fileName = FileValidator.Save(_environment.WebRootPath, "uploads/Collaboration", Collaboration.ImageFile);
            }

            if (fileName != null || Collaboration.Image == null)
            {
                if (existCollaboration.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/Collaboration", existCollaboration.Image);
                }

                existCollaboration.Image = fileName;
            }

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Collaboration Collaboration = _datacontext.Collaborations.FirstOrDefault(x => x.Id == id);
            if (Collaboration == null) return Json(new { status = 404 });

            try
            {
                if (Collaboration.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/Collaboration", Collaboration.Image);
                }

                _datacontext.Collaborations.Remove(Collaboration);
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