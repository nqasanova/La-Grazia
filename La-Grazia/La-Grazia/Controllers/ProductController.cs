using System;
using La_Grazia.ViewModels;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using La_Grazia.Database;
using Microsoft.EntityFrameworkCore;

namespace La_Grazia.Areas.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductController(DataContext dataContext, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index(int page = 1, int? cheapPriceId = null, int? middlePriceId = null, int? expensivePriceId = null,
                                   int? regionId = null, int? typeId = null, int? varietyId = null, string search = null)
        {
            if (page <= 0)
            {
                page = 1;
            }

            ViewBag.Wishlists = null;

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

                }
            }

            else
            {
                List<WishlistProduct> WishlistProducts = _dataContext.WishlistProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.UserId == member.Id).ToList();
                items = WishlistProducts.Select(x => new WishlistProductViewModel
                {
                    ProductId = x.ProductId,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            ViewBag.Wishlists = items;

            ViewBag.CurrentRegionId = regionId;
            ViewBag.CurrentTypeId = typeId;
            ViewBag.CurrentVarietyId = varietyId;
            ViewBag.CurrentSearch = search;

            var query = _dataContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(x => x.Name.Contains(search));

            if (regionId != null)
                query = query.Where(x => x.RegionId == regionId);


            if (typeId != null)
                query = query.Where(x => x.TypeId == typeId);

            if (varietyId != null)
                query = query.Where(x => x.VarietyId == varietyId);

            if (cheapPriceId != null)
                query = query.Where(x => x.SalePrice <= 100);

            if (middlePriceId != null)
                query = query.Where(x => x.SalePrice > 100 && x.SalePrice < 300);

            if (expensivePriceId != null)
                query = query.Where(x => x.SalePrice >= 300);


            ProductViewModel productVM = new ProductViewModel
            {
                Varieties = _dataContext.Varieties.Include(x => x.Products).ToList(),
                Types = _dataContext.Types.Include(x => x.Products).ToList(),
                Regions = _dataContext.Regions.Include(x => x.Products).ToList(),
                Products = query.Include(x => x.ProductImages).Skip((page - 1) * 9).Take(9).ToList()
            };

            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(query.Count() / 9m);

            return View(productVM);
        }

        public IActionResult Detail(int id)
        {
            if (!ModelState.IsValid) return RedirectToAction("index", "error");

            ViewBag.Wishlists = null;

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

                }
            }
            else
            {
                List<WishlistProduct> WishlistProducts = _dataContext.WishlistProducts.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.UserId == member.Id).ToList();
                items = WishlistProducts.Select(x => new WishlistProductViewModel
                {
                    ProductId = x.ProductId,
                    Image = x.Product.ProductImages.FirstOrDefault()?.Image,
                    Name = x.Product.Name,
                    Price = x.Product.SalePrice
                }).ToList();
            }

            ViewBag.Wishlists = items;

            ViewBag.SameProducts = _dataContext.Products.Include(x => x.ProductImages).Where(x => x.Rate > 4).Take(4).ToList();

            Product product = _dataContext.Products.Include(x => x.ProductImages).Include(x => x.Variety).Include(x => x.Type)
                .Include(x => x.Region).FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return RedirectToAction("index", "error");
            }

            List<Review> reviews = _dataContext.Reviews.Include(x => x.User).Where(x => x.IsAccepted && x.ProductId == id).ToList();

            ReviewViewModel reviewVM = new ReviewViewModel
            {
                Product = product,
                Reviews = reviews
            };

            return View(reviewVM);
        }


        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(int id, ReviewViewModel model)
        {

            Product product = _dataContext.Products.Find(id);

            if (product == null) return RedirectToAction("index", "error");

            User member = await _userManager.FindByNameAsync(User.Identity.Name);

            if (model.Context.Length > 250)
            {
                ModelState.AddModelError("Text", "Your text is very length");
                return RedirectToAction("index", "error");
            }

            Review review = new Review
            {
                UserId = member.Id,
                ProductId = id,
                Rate = model.Rate,
                Context = model.Context,
                CreatedAt = DateTime.Now
            };


            _dataContext.Reviews.Add(review);

            await _dataContext.SaveChangesAsync();

            return Redirect(HttpContext.Request.Headers["Referer"].ToString());
        }


        public IActionResult AddToBasket(int id)
        {
            Product product = _dataContext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
            BasketProductViewModel BasketProduct = null;

            if (product == null) return RedirectToAction("index", "error");

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            List<BasketProductViewModel> products = new List<BasketProductViewModel>();

            if (member == null)
            {
                string productsStr;

                if (HttpContext.Request.Cookies["Products"] != null)
                {
                    productsStr = HttpContext.Request.Cookies["Products"];
                    products = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productsStr);

                    BasketProduct = products.FirstOrDefault(x => x.ProductId == id);
                }

                if (BasketProduct == null)
                {
                    BasketProduct = new BasketProductViewModel
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        Image = product.ProductImages.FirstOrDefault().Image,
                        Price = product.SalePrice,
                        Count = 1
                    };
                    products.Add(BasketProduct);
                }
                else
                {
                    BasketProduct.Count++;
                }
                productsStr = JsonConvert.SerializeObject(products);
                HttpContext.Response.Cookies.Append("Products", productsStr);
            }
            else
            {
                BasketProduct memberBasketProduct = _dataContext.BasketProducts.FirstOrDefault(x => x.UserId == member.Id && x.ProductId == id);

                if (memberBasketProduct == null)
                {
                    memberBasketProduct = new BasketProduct
                    {
                        UserId = member.Id,
                        ProductId = id,
                        Count = 1
                    };
                    _dataContext.BasketProducts.Add(memberBasketProduct);
                }
                else
                {
                    memberBasketProduct.Count++;
                }
                _dataContext.SaveChanges();

                products = _dataContext.BasketProducts.Include(x => x.Product).ThenInclude(pi => pi.ProductImages)
                    .Where(x => x.UserId == member.Id)
                    .Select(x => new BasketProductViewModel
                    {
                        ProductId = x.ProductId,
                        Count = x.Count,
                        Name = x.Product.Name,
                        Price = x.Product.SalePrice,
                        Image = x.Product.ProductImages.FirstOrDefault().Image
                    })
                    .ToList();
            }

            return PartialView("_BasketPartial", products);
        }


        public IActionResult DeleteBasketProduct(int id)
        {
            Product product = _dataContext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

            BasketProductViewModel BasketProduct = null;

            if (product == null) return RedirectToAction("index", "error");

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            List<BasketProductViewModel> products = new List<BasketProductViewModel>();

            if (member == null)
            {

                string productStr = HttpContext.Request.Cookies["Products"];
                products = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productStr);

                BasketProduct = products.FirstOrDefault(x => x.ProductId == id);


                if (BasketProduct.Count == 1)
                {

                    BasketProduct.Count = 1;
                }
                else
                {
                    BasketProduct.Count--;
                }
                productStr = JsonConvert.SerializeObject(products);
                HttpContext.Response.Cookies.Append("Products", productStr);
            }

            else
            {
                BasketProduct memberBasketProduct = _dataContext.BasketProducts.Include(x => x.Product).FirstOrDefault(x => x.UserId == member.Id && x.ProductId == id);

                if (memberBasketProduct.Count == 1)
                {

                    memberBasketProduct.Count = 1;
                }
                else
                {
                    memberBasketProduct.Count--;
                }

                _dataContext.SaveChanges();

                products = _dataContext.BasketProducts.Include(x => x.Product).ThenInclude(pi => pi.ProductImages).Where(x => x.UserId == member.Id)
                    .Select(x => new BasketProductViewModel
                    {
                        ProductId = x.ProductId,
                        Count = x.Count,
                        Name = x.Product.Name,
                        Price = x.Product.SalePrice,
                        Image = x.Product.ProductImages.FirstOrDefault().Image
                    })
                    .ToList();

            }
            return PartialView("_BasketPartial", products);
        }

        public IActionResult RemoveBasketProduct(int id)
        {
            Product product = _dataContext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

            BasketProductViewModel BasketProduct = null;

            if (product == null) return RedirectToAction("index", "error");

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            List<BasketProductViewModel> products = new List<BasketProductViewModel>();

            if (member == null)
            {

                string productStr = HttpContext.Request.Cookies["Products"];
                products = JsonConvert.DeserializeObject<List<BasketProductViewModel>>(productStr);

                BasketProduct = products.FirstOrDefault(x => x.ProductId == id);


                products.Remove(BasketProduct);

                productStr = JsonConvert.SerializeObject(products);

                HttpContext.Response.Cookies.Append("Products", productStr);
            }

            else
            {
                BasketProduct memberBasketProduct = _dataContext.BasketProducts.Include(x => x.Product).FirstOrDefault(x => x.UserId == member.Id && x.ProductId == id);

                _dataContext.BasketProducts.Remove(memberBasketProduct);

                _dataContext.SaveChanges();

                products = _dataContext.BasketProducts.Include(x => x.Product).ThenInclude(pi => pi.ProductImages).Where(x => x.UserId == member.Id)
                    .Select(x => new BasketProductViewModel
                    {
                        ProductId = x.ProductId,
                        Count = x.Count,
                        Name = x.Product.Name,
                        Price = x.Product.SalePrice,
                        Image = x.Product.ProductImages.FirstOrDefault().Image
                    })
                    .ToList();

            }
            return PartialView("_BasketPartial", products);
        }


        public IActionResult AddToWishlist(int id)
        {

            Product product = _dataContext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

            WishlistProductViewModel WishlistProduct = null;

            if (product == null) return RedirectToAction("index", "error");

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            List<WishlistProductViewModel> products = new List<WishlistProductViewModel>();

            if (member == null)
            {
                string productStr;

                if (HttpContext.Request.Cookies["Wishlist"] != null)
                {
                    productStr = HttpContext.Request.Cookies["Wishlist"];

                    products = JsonConvert.DeserializeObject<List<WishlistProductViewModel>>(productStr);

                    WishlistProduct = products.FirstOrDefault(x => x.ProductId == id);
                }

                if (WishlistProduct == null)
                {
                    WishlistProduct = new WishlistProductViewModel
                    {
                        ProductId = product.Id,
                        Name = product.Name,
                        Image = product.ProductImages.FirstOrDefault().Image,
                        Price = (product.SalePrice),
                    };
                    products.Add(WishlistProduct);
                }

                else
                {
                    products.Remove(WishlistProduct);
                }
                productStr = JsonConvert.SerializeObject(products);

                HttpContext.Response.Cookies.Append("Wishlist", productStr);
            }

            else
            {
                WishlistProduct memberWishlistProduct = _dataContext.WishlistProducts.FirstOrDefault(x => x.UserId == member.Id && x.ProductId == id);
                if (memberWishlistProduct == null)
                {
                    memberWishlistProduct = new WishlistProduct
                    {
                        UserId = member.Id,
                        ProductId = id,

                    };
                    _dataContext.WishlistProducts.Add(memberWishlistProduct);
                }
                else
                {
                    _dataContext.WishlistProducts.Remove(memberWishlistProduct);
                }

                _dataContext.SaveChanges();

                products = _dataContext.WishlistProducts.Include(x => x.Product).ThenInclude(bi => bi.ProductImages)
                    .Where(x => x.UserId == member.Id)
                    .Select(x => new WishlistProductViewModel
                    {
                        ProductId = x.ProductId,
                        Name = x.Product.Name,
                        Price = x.Product.SalePrice,
                        Image = x.Product.ProductImages.FirstOrDefault().Image
                    })
                    .ToList();
            }

            return RedirectToAction("index", "wishlist", products);
        }


        public IActionResult DeleteWishlistProduct(int id)
        {
            Product product = _dataContext.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);

            WishlistProductViewModel WishlistProduct = null;

            if (product == null) return RedirectToAction("index", "error");

            User member = null;

            if (User.Identity.IsAuthenticated)
            {
                member = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && !x.IsAdmin);

            }

            List<WishlistProductViewModel> products = new List<WishlistProductViewModel>();

            if (member == null)
            {
                string productStr = HttpContext.Request.Cookies["Wishlist"];

                products = JsonConvert.DeserializeObject<List<WishlistProductViewModel>>(productStr);

                WishlistProduct = products.FirstOrDefault(x => x.ProductId == id);

                products.Remove(WishlistProduct);

                productStr = JsonConvert.SerializeObject(products);
                HttpContext.Response.Cookies.Append("Wishlist", productStr);
            }

            else
            {
                WishlistProduct memberWishlistProduct = _dataContext.WishlistProducts.Include(x => x.Product).FirstOrDefault(x => x.UserId == member.Id && x.ProductId == id);

                _dataContext.WishlistProducts.Remove(memberWishlistProduct);

                _dataContext.SaveChanges();

                products = _dataContext.WishlistProducts.Include(x => x.Product).ThenInclude(bi => bi.ProductImages).Where(x => x.UserId == member.Id)
                    .Select(x => new WishlistProductViewModel { ProductId = x.ProductId, Name = x.Product.Name, Price = x.Product.SalePrice, Image = x.Product.ProductImages.FirstOrDefault().Image }).ToList();

            }

            return RedirectToAction("index", "wishlist", products);
        }
    }
}