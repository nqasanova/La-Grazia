//using System;
//using La_Grazia.Areas.Admin.ViewModels;
//using La_Grazia.Database.Models;
//using La_Grazia.Database.Models.Enums;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using La_Grazia.Database;
//using Microsoft.EntityFrameworkCore;

//namespace La_Grazia.Areas.Admin.Controllers
//{
//    [Area("manage")]
//    [Authorize(Roles = "Admin,SuperAdmin")]
//    public class DashboardController : Controller
//    {
//        private readonly DataContext _datacontext;

//        public DashboardController(DataContext context)
//        {
//            _datacontext = context;
//        }

//        public IActionResult Index()
//        {
//            DashboardViewModel dashboardVM = new DashboardViewModel
//            {
//                AcceptedOrders = _datacontext.Orders.Where(x => x.Status == OrderStatus.Approved).Include(x => x.OrderedProducts).ToList(),
//                PendingOrders = _datacontext.Orders.Where(x => x.Status == OrderStatus.Pending).Include(x => x.OrderedProducts).ToList(),
//                Users = _datacontext.Users.ToList(),
//                LatestOrders = _datacontext.Orders.OrderByDescending(x => x.CreatedAt).Take(5).ToList(),
//                SaturdayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 1 && x.Status == OrderStatus.Approved).Count(),
//                SaturdayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 1 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                SundayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 2 && x.Status == OrderStatus.Approved).Count(),
//                SundayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 2 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                MondayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 3 && x.Status == OrderStatus.Approved).Count(),
//                MondayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 3 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                TuesdayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 4 && x.Status == OrderStatus.Approved).Count(),
//                TuesdayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 4 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                WednesdayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 5 && x.Status == OrderStatus.Approved).Count(),
//                WednesdayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 5 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                ThursdayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 6 && x.Status == OrderStatus.Approved).Count(),
//                ThursdayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 6 && x.Status == OrderStatus.Approved).Sum(x => x.Total),
//                FridayOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day == 7 && x.Status == OrderStatus.Approved).Count(),
//                FridayIncome = _datacontext.Orders.Where(x => x.CreatedAt.Day == 7 && x.Status == OrderStatus.Approved).Sum(x => x.Total)
//            };

//            double redWinesCount = _datacontext.Products.Where(x => x.Type.Name == "Red").Count();
//            double whiteWinesCount = _datacontext.Products.Where(x => x.Type.Name == "White").Count();
//            double roseWinesCount = _datacontext.Products.Where(x => x.Type.Name == "Rose").Count();
//            double totalCount = _datacontext.Products.Count();
//            ViewBag.RedWinesPercent = Math.Ceiling(redWinesCount / totalCount * 100);
//            ViewBag.WhiteWinesPercent = Math.Ceiling(whiteWinesCount / totalCount * 100);
//            ViewBag.RoseWinesPercent = Math.Ceiling(roseWinesCount / totalCount * 100);

//            double weeklyIncome = 0;
//            List<Order> orders = _datacontext.Orders.Where(x => x.CreatedAt.Day >= 1 && x.CreatedAt.Day <= 7 && x.Status == OrderStatus.Approved).Include(x => x.OrderedProducts).ToList();
//            foreach (var item in orders)
//            {
//                weeklyIncome += (item.OrderedProducts.Sum(x => x.SalePrice * x.Count) - item.OrderedProducts.Sum(x => x.Price * x.Count));
//            }

//            ViewBag.WeeklyOrders = _datacontext.Orders.Where(x => x.CreatedAt.Day >= 1 && x.CreatedAt.Day <= 7 && x.Status == OrderStatus.Approved).Count();
//            ViewBag.WeeklyIncome = weeklyIncome;

//            return View(dashboardVM);
//        }
//    }
//}