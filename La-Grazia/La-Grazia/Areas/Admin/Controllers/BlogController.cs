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
    public class BlogController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _env;

        public BlogController(DataContext datacontext, IWebHostEnvironment env)
        {
            _datacontext = datacontext;
            _env = env;
        }

        #region Index

        public IActionResult Index()
        {
            List<Blog> blogs = _datacontext.Blogs.ToList();
            return View(blogs);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (blog.ImageFile != null)
            {
                if (blog.ImageFile.ContentType != "image/jpeg" && blog.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Content type can only be jpeg or png!");
                    return View();
                }

                if (blog.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2mb!");
                    return View();
                }

                string fileName = FileValidator.Save(_env.WebRootPath, "uploads/blog", blog.ImageFile);

                blog.Image = fileName;
            }

            if (blog.Order < 0)
            {
                ModelState.AddModelError("Order", "Order cannot be less than 0!");
                return View();
            }

            _datacontext.Blogs.Add(blog);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Blog blog = _datacontext.Blogs.FirstOrDefault(x => x.Id == id);

            if (blog == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            return View(blog);
        }

        [HttpPost]
        public IActionResult Edit(Blog blog)
        {
            if (!ModelState.IsValid) return View();

            Blog existBlog = _datacontext.Blogs.FirstOrDefault(x => x.Id == blog.Id);

            if (existBlog == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            string fileName = null;

            if (blog.ImageFile != null)
            {
                if (blog.ImageFile.ContentType != "image/png" && blog.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type can only be jpeg,jpg or png!");
                    return View(existBlog);
                }

                if (blog.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size cannot be more than 2MB!");
                    return View(existBlog);
                }

                fileName = FileValidator.Save(_env.WebRootPath, "uploads/blog", blog.ImageFile);
            }

            if (fileName != null || blog.Image == null)
            {
                if (existBlog.Image != null)
                {
                    FileValidator.Delete(_env.WebRootPath, "uploads/blog", existBlog.Image);
                }

                existBlog.Image = fileName;
            }

            if (blog.Order < 0)
            {
                ModelState.AddModelError("Order", "Order can not be less than 0");
                return View();
            }

            existBlog.Order = blog.Order;
            existBlog.Name = blog.Name;
            existBlog.Owner = blog.Owner;
            existBlog.Date = blog.Date;
            existBlog.Description1 = blog.Description1;
            existBlog.Description2 = blog.Description2;
            existBlog.Description3 = blog.Description3;
            existBlog.Description4 = blog.Description4;
            existBlog.Description5 = blog.Description5;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Blog blog = _datacontext.Blogs.FirstOrDefault(x => x.Id == id);

            if (blog == null) return Json(new { status = 404 });

            try
            {
                if (blog.Image != null)
                {
                    FileValidator.Delete(_env.WebRootPath, "uploads/blog", blog.Image);
                }

                _datacontext.Blogs.Remove(blog);
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