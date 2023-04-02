using System;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;
using Microsoft.EntityFrameworkCore;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class ReviewController : Controller
    {
        private readonly DataContext _datacontext;

        public ReviewController(DataContext context)
        {
            _datacontext = context;
        }

        #region Index

        public IActionResult Index()
        {
            List<Review> reviews = _datacontext.Reviews.Include(x => x.User).ToList();
            return View(reviews);
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Review review = _datacontext.Reviews.Include(x => x.Product).Include(x => x.User).FirstOrDefault(x => x.Id == id);

            return View(review);
        }

        #endregion

        #region Accept

        public IActionResult Accept(int id)
        {
            Review review = _datacontext.Reviews.FirstOrDefault(x => x.Id == id);
            if (review == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            review.IsAccepted = true;

            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Reject

        public IActionResult Reject(int id)
        {
            Review review = _datacontext.Reviews.FirstOrDefault(x => x.Id == id);
            if (review == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            review.IsAccepted = false;

            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion
    }
}