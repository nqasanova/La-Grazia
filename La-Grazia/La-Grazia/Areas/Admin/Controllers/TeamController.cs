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
    public class TeamController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _environment;

        public TeamController(DataContext datacontext, IWebHostEnvironment environment)
        {
            _datacontext = datacontext;
            _environment = environment;
        }

        #region Index

        public IActionResult Index()
        {
            List<Team> teams = _datacontext.Teams.ToList();
            return View(teams);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (team.ImageFile != null)
            {
                if (team.ImageFile.ContentType != "image/jpeg" && team.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type can only be jpeg or png!");
                    return View();
                }

                if (team.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2mb!");
                    return View();
                }

                string fileName = FileValidator.Save(_environment.WebRootPath, "uploads/team", team.ImageFile);

                team.Image = fileName;
            }

            _datacontext.Teams.Add(team);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Team team = _datacontext.Teams.FirstOrDefault(x => x.Id == id);
            if (team == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(team);
        }

        [HttpPost]
        public IActionResult Edit(Team team)
        {
            if (!ModelState.IsValid) return View();

            Team existTeam = _datacontext.Teams.FirstOrDefault(x => x.Id == team.Id);
            if (existTeam == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            string fileName = null;

            if (team.ImageFile != null)
            {
                if (team.ImageFile.ContentType != "image/png" && team.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type can only be jpeg,jpg or png!");
                    return View(existTeam);
                }

                if (team.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2MB!");
                    return View(existTeam);
                }

                fileName = FileValidator.Save(_environment.WebRootPath, "uploads/team", team.ImageFile);
            }

            if (fileName != null || team.Image == null)
            {
                if (existTeam.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/team", existTeam.Image);
                }

                existTeam.Image = fileName;
            }

            existTeam.FullName = team.FullName;
            existTeam.Profession = team.Profession;
            existTeam.InstagramURL = team.InstagramURL;
            existTeam.TwitterURL = team.TwitterURL;
            existTeam.FacebookURL = team.FacebookURL;
            existTeam.PinterestURL = team.PinterestURL;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Team team = _datacontext.Teams.FirstOrDefault(x => x.Id == id);
            if (team == null) return Json(new { status = 404 });

            try
            {
                if (team.Image != null)
                {
                    FileValidator.Delete(_environment.WebRootPath, "uploads/team", team.Image);
                }

                _datacontext.Teams.Remove(team);
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