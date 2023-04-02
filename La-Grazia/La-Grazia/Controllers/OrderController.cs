using System;
using Azure;
using La_Grazia.ViewModels;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace La_Grazia.Areas.Controllers
{
    public class OrderController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(DataContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            CheckoutViewModel checkoutVM = new CheckoutViewModel();

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                var productStr = HttpContext.Request.Cookies["Products"];

                if (!string.IsNullOrWhiteSpace(productStr))
                {
                    checkoutVM.BasketProductViewModels = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productStr);

                    foreach (var item in checkoutVM.BasketProductViewModels)
                    {
                        Product product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);

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
                checkoutVM.Email = member.Email;
                checkoutVM.FullName = member.FullName;
                checkoutVM.Phone = member.PhoneNumber;

                checkoutVM.BasketProductViewModels = _context.BasketProducts.Include(x => x.Product).Where(x => x.UserId == member.Id)
                                                   .Select(x => new BasketProductViewModel
                                                   {
                                                       ProductId = x.ProductId,
                                                       Name = x.Product.Name,
                                                       Count = x.Count,
                                                       Price = x.Product.SalePrice
                                                   }).ToList();

            }

            return View(checkoutVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutViewModel checkoutVM)
        {
            TempData["Order"] = false;

            checkoutVM.BasketProductViewModels = _getBasketProducts();

            if (!ModelState.IsValid) return View(checkoutVM);

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            Order order = new Order
            {
                FullName = checkoutVM.FullName,
                Email = checkoutVM.Email,
                Address = checkoutVM.Address,
                City = checkoutVM.City,
                Note = checkoutVM.Note,
                Phone = checkoutVM.Phone,
                Status = La_Grazia.Database.Models.Enums.OrderStatus.Pending,
                CreatedAt = DateTime.Now,
                OrderedProducts = new List<OrderedProduct>()
            };

            List<BasketProductViewModel> BasketProductVMs = new List<BasketProductViewModel>();

            if (member == null)
            {
                var productStr = HttpContext.Request.Cookies["Products"];

                if (productStr != null)
                {
                    BasketProductVMs = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productStr);
                }
            }
            else
            {
                order.UserId = member.Id;
                BasketProductVMs = _context.BasketProducts.Where(x => x.UserId == member.Id).Select(x => new BasketProductViewModel { ProductId = x.ProductId, Count = x.Count }).ToList();
            }

            foreach (var item in BasketProductVMs)
            {
                Product product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);

                if (product == null)
                {
                    ModelState.AddModelError("", "Selected product not found");
                    return View(checkoutVM);
                }

                _addOrderedProduct(ref order, product, item.Count);
            }

            //if (order.OrderedProducts.Count == 0)
            //{
            //    ModelState.AddModelError("", "There is not any selected product!");
            //    return View(checkoutVM);
            //}

            _context.Orders.Add(order);
            _context.SaveChanges();

            if (member == null)
            {
                Response.Cookies.Delete("Products");
            }
            else
            {
                _context.BasketProducts.RemoveRange(_context.BasketProducts.Where(x => x.UserId == member.Id));
                _context.SaveChanges();
            }

            TempData["Order"] = true;

            return RedirectToAction("index", "home");
        }


        private void _addOrderedProduct(ref Order order, Product product, int count)
        {
            OrderedProduct OrderedProduct = new OrderedProduct
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Count = count,
                ProductImage = product.ProductImages.FirstOrDefault().Image
            };

            //order.OrderedProducts.Add(OrderedProduct);
            order.Total += OrderedProduct.SalePrice * OrderedProduct.Count;
        }
        private List<BasketProductViewModel> _getBasketProducts()
        {
            List<BasketProductViewModel> BasketProducts = new List<BasketProductViewModel>();

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);
            }

            if (member == null)
            {
                var productsStr = HttpContext.Request.Cookies["Products"];
                if (!string.IsNullOrWhiteSpace(productsStr))
                {
                    BasketProducts = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productsStr);

                    foreach (var item in BasketProducts)
                    {
                        Product product = _context.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == item.ProductId);
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

                BasketProducts = _context.BasketProducts.Include(x => x.Product).Where(x => x.UserId == member.Id)
                                                    .Select(x => new BasketProductViewModel
                                                    {
                                                        ProductId = x.ProductId,
                                                        Name = x.Product.Name,
                                                        Count = x.Count,
                                                        Price = x.Product.SalePrice
                                                    }).ToList();

            }
            return BasketProducts;
        }
    }
}