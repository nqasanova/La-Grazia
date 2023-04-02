using System;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;
using Microsoft.EntityFrameworkCore;
using La_Grazia.Validators;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProductController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IWebHostEnvironment _environment;

        public ProductController(DataContext context, IWebHostEnvironment environment)
        {
            _datacontext = context;
            _environment = environment;
        }

        #region Index

        public IActionResult Index()
        {
            List<Product> products = _datacontext.Products.Include(x => x.Type).Include(x => x.Variety).Include(x => x.ProductImages).ToList();
            return View(products);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            ViewBag.Types = _datacontext.Types.ToList();
            ViewBag.Regions = _datacontext.Regions.ToList();
            ViewBag.Varieties = _datacontext.Varieties.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            ViewBag.Types = _datacontext.Types.ToList();
            ViewBag.Regions = _datacontext.Regions.ToList();
            ViewBag.Variaties = _datacontext.Varieties.ToList();

            if (!_datacontext.Types.Any(x => x.Id == product.TypeId)) ModelState.AddModelError("TypeId", "Type is not found!");
            if (!_datacontext.Regions.Any(x => x.Id == product.RegionId)) ModelState.AddModelError("RegionId", "Region is not found!");
            if (!_datacontext.Varieties.Any(x => x.Id == product.VarietyId)) ModelState.AddModelError("VarietyId", "Variety is not found!");

            if (!ModelState.IsValid) return View();

            product.ProductImages = new List<ProductImage>();

            if (product.ImageFiles != null)
            {
                foreach (var file in product.ImageFiles)
                {
                    if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
                    {
                        continue;
                    }

                    if (file.Length > 2097152)
                    {
                        continue;
                    }

                    string newFileName = FileValidator.Save(_environment.WebRootPath, "uploads/product", file);

                    ProductImage image = new ProductImage
                    {
                        Image = newFileName
                    };

                    product.ProductImages.Add(image);
                }
            }


            if (product.Rate < 0)
            {
                ModelState.AddModelError("Rate", "Rate count cannot be less than 0");
                return View();
            }

            if (product.Price < 0)
            {
                ModelState.AddModelError("CostPrice", "CostPrice count cannot be less than 0");
                return View();
            }

            if (product.SalePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "SalePrice count cannot be less than 0");
                return View();
            }

            _datacontext.Products.Add(product);
            _datacontext.SaveChanges();

            return RedirectToAction("index");
        }

        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {
            Product product = _datacontext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            ViewBag.Types = _datacontext.Types.ToList();
            ViewBag.Regions = _datacontext.Regions.ToList();
            ViewBag.Variaties = _datacontext.Varieties.ToList();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            ViewBag.Types = _datacontext.Types.ToList();
            ViewBag.Regions = _datacontext.Regions.ToList();
            ViewBag.Variaties = _datacontext.Varieties.ToList();

            if (!_datacontext.Types.Any(x => x.Id == product.TypeId)) ModelState.AddModelError("TypeId", "Type not found!");
            if (!_datacontext.Regions.Any(x => x.Id == product.RegionId)) ModelState.AddModelError("RegionId", "Region not found!");
            if (!_datacontext.Varieties.Any(x => x.Id == product.VarietyId)) ModelState.AddModelError("VariatyId", "Variety not found!");

            if (!ModelState.IsValid) return View();

            Product existProduct = _datacontext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == product.Id);
            if (existProduct == null)
            {
                return RedirectToAction("index", "error", new { area = "" });
            }

            List<ProductImage> removablePhotos = existProduct.ProductImages.ToList();

            foreach (var item in removablePhotos)
            {
                FileValidator.Delete(_environment.WebRootPath, "uploads/product", item.Image);
            }

            //existProduct.ProductImages.RemoveAll(!product.ProductImageIds.Contains(Id);

            if (product.ImageFiles != null)
            {
                foreach (var file in product.ImageFiles)
                {
                    if (file.ContentType != "image/png" && file.ContentType != "image/jpeg")
                    {
                        continue;
                    }

                    if (file.Length > 2097152)
                    {
                        continue;
                    }

                    string newFileName = FileValidator.Save(_environment.WebRootPath, "uploads/product", file);

                    ProductImage image = new ProductImage
                    {
                        Image = newFileName
                    };

                    existProduct.ProductImages.Add(image);
                }
            }

            if (product.Rate < 0)
            {
                ModelState.AddModelError("Rate", "Rate count can not be less than 0");
                return View(existProduct);
            }

            if (product.Price < 0)
            {
                ModelState.AddModelError("CostPrice", "CostPrice count can not be less than 0");
                return View(existProduct);
            }

            if (product.SalePrice < 0)
            {
                ModelState.AddModelError("SalePrice", "SalePrice count can not be less than 0");
                return View(existProduct);
            }

            existProduct.Name = product.Name;
            existProduct.VarietyId = product.VarietyId;
            existProduct.TypeId = product.TypeId;
            existProduct.RegionId = product.RegionId;
            existProduct.SalePrice = product.SalePrice;
            existProduct.Price = product.Price;
            existProduct.IsFeatured = product.IsFeatured;
            existProduct.Rate = product.Rate;
            existProduct.Description = product.Description;

            _datacontext.SaveChanges();
            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public IActionResult DeleteFetch(int id)
        {
            Product product = _datacontext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return Json(new { status = 404 });

            try
            {
                _datacontext.Products.Remove(product);
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