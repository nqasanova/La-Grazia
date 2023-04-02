using System;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataContext _datacontext;

        public BlogController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        #region Index

        public IActionResult Index()
        {
            List<Blog> blogs = _datacontext.Blogs.ToList();
            return View(blogs);
        }

        #endregion

        #region Detail

        public IActionResult Detail(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("index", "error");
            }

            Blog blog = _datacontext.Blogs.FirstOrDefault(x => x.Id == id);

            return View(blog);
        }

        #endregion
    }
}