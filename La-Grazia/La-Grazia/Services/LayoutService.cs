using System;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using La_Grazia.ViewModels;
using Newtonsoft.Json;

namespace La_Grazia.Services
{
    public class LayoutService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<User> _userManager;

        public LayoutService(DataContext dataContext, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public Setting GetSetting()
        {
            return _dataContext.Settings.FirstOrDefault();
        }

        public List<BasketProductViewModel> GetBasketProducts()
        {
            List<BasketProductViewModel> items = new List<BasketProductViewModel>();

            User member = null;

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == _contextAccessor.HttpContext.User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                var itemsStr = _contextAccessor.HttpContext.Request.Cookies["Products"];

                if (itemsStr != null)
                {
                    items = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(itemsStr);

                    foreach (var item in items)
                    {
                        Product product = _dataContext.Products.Include(c => c.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);
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
                List<BasketProduct> BasketProducts = _dataContext.BasketProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.UserId == member.Id).ToList();
                items = BasketProducts.Select(x => new BasketProductViewModel
                {
                    ProductId = x.ProductId,
                    Count = x.Count,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            return items;
        }

        public List<WishlistProductViewModel> GetWishlistItems()
        {
            List<WishlistProductViewModel> items = new List<WishlistProductViewModel>();

            User member = null;

            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == _contextAccessor.HttpContext.User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                var itemsStr = _contextAccessor.HttpContext.Request.Cookies["Wishlist"];

                if (itemsStr != null)
                {
                    items = JsonConvert.DeserializeObject<List<WishlistProductViewModel>>(itemsStr);

                    foreach (var item in items)
                    {
                        Product product = _dataContext.Products.Include(c => c.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);

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
                List<WishlistProduct> wishlistItems = _dataContext.WishlistProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.UserId == member.Id).ToList();
                items = wishlistItems.Select(x => new WishlistProductViewModel
                {
                    ProductId = x.ProductId,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            return items;
        }
    }
}