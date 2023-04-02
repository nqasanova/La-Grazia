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
    public class CartController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly IHttpContextAccessor _datacontextAccessor;
        private readonly UserManager<User> _userManager;

        public CartController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        #region Index

        public IActionResult Index()
        {
            List<BasketProductViewModel> items = new List<BasketProductViewModel>();

            User member = null;

            if (_datacontextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == _datacontextAccessor.HttpContext.User.Identity.Name);
            }

            if (member == null)
            {
                var itemsStr = _datacontextAccessor.HttpContext.Request.Cookies["Products"];

                if (itemsStr != null)
                {
                    items = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(itemsStr);

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
                List<BasketProduct> BasketProducts = _datacontext.BasketProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.UserId == member.Id).ToList();

                items = BasketProducts.Select(x => new BasketProductViewModel
                {
                    ProductId = x.ProductId,
                    Count = x.Count,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            return View(items);
        }

        #endregion
    }
}