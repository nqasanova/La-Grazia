using System;
using La_Grazia.ViewModels;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace La_Grazia.Areas.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;

        public HomeController(DataContext datacontext, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            _datacontext = datacontext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        #region Index

        public IActionResult Index()
        {
            ViewBag.Wishlists = null;

            List<WishlistProductViewModel> items = new List<WishlistProductViewModel>();

            User member = null;

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.Email == _contextAccessor.HttpContext.User.Identity.Name);
            }

            if (member == null)
            {
                var itemsStr = _contextAccessor.HttpContext.Request.Cookies["Wishlist"];

                if (itemsStr != null)
                {
                    items = JsonConvert.DeserializeObject<List<WishlistProductViewModel>>(itemsStr);
                }
            }

            else
            {
                List<WishlistProduct> WishlistProducts = _datacontext.WishlistProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).ToList(); //.Where(x => x.UserId == member.Id).ToList();
                items = WishlistProducts.Select(x => new WishlistProductViewModel
                {
                    ProductId = x.ProductId,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            ViewBag.Wishlists = items;

            HomeViewModel homeVM = new HomeViewModel
            {
                Sliders = _datacontext.Sliders.ToList(),
                Blogs = _datacontext.Blogs.Take(4).ToList(),
                FeaturedProducts = _datacontext.Products.Include(x => x.ProductImages).Where(x => x.IsFeatured).Take(8).ToList(),
                PopularProducts = _datacontext.Products.Include(x => x.ProductImages).Where(x => x.Rate > 4).Take(8).ToList(),
                Collaborations = _datacontext.Collaborations.ToList(),
                Reviews = _datacontext.Reviews.Include(x => x.User).ToList(),
            };

            return View(homeVM);
        }

        #endregion

        #region Get Product

        public IActionResult GetProduct(int id)
        {
            Product product = _datacontext.Products
                .Include(x => x.ProductImages).Include(x => x.Variety)
                .Include(x => x.Type)
                .Include(x => x.Region)
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_ProductModalPartial", product);
        }

        #endregion

        #region Search

        public IActionResult Search(string search)
        {
            var query = _datacontext.Products.Include(x => x.ProductImages).AsQueryable().Where(x => x.Name.Contains(search));

            List<Product> products = query.OrderByDescending(x => x.Id).ToList();

            return PartialView("_SearchPartial", products);
        }

        #endregion
    }
}