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
    public class WishlistController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;

        public WishlistController(DataContext datacontext, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            _datacontext = datacontext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        #region Index

        public IActionResult Index()
        {
            List<WishlistProductViewModel> items = new List<WishlistProductViewModel>();

            User member = null;

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == _contextAccessor.HttpContext.User.Identity.Name);
            }

            if (member == null)
            {
                var itemsStr = _contextAccessor.HttpContext.Request.Cookies["Wishlist"];

                if (itemsStr != null)
                {
                    items = JsonConvert.DeserializeObject<List<WishlistProductViewModel>>(itemsStr);

                    foreach (var item in items)
                    {
                        Product product = _datacontext.Products.Include(c => c.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);

                        if (product != null)
                        {
                            item.Name = product.Name;
                            item.Price = product.SalePrice;
                            item.Image = product.ProductImages.FirstOrDefault()?.Image;
                        }
                    }
                }
            }

            else
            {
                List<WishlistProduct> wishlistItems = _datacontext.WishlistProducts.
                    Include(x => x.Product)
                    .ThenInclude(x => x.ProductImages)
                    .Where(x => x.UserId == member.Id)
                    .ToList();

                items = wishlistItems.Select(x => new WishlistProductViewModel
                {
                    Price = x.Product.SalePrice,
                    ProductId = x.ProductId,
                    Name = x.Product.Name,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image
                }).ToList();
            }

            return View(items);
        }

        #endregion
    }
}