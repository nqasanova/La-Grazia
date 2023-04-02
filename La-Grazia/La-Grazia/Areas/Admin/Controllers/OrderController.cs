//using System;
//using La_Grazia.Database.Models;
//using La_Grazia.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using La_Grazia.Database;
//using Microsoft.EntityFrameworkCore;

//namespace La_Grazia.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Roles = "Admin,SuperAdmin")]
//    public class OrderController : Controller
//    {
//        private readonly DataContext _datacontext;
//        private readonly IEmailService _emailService;

//        public OrderController(DataContext context, IEmailService emailService)
//        {
//            _datacontext = context;
//            _emailService = emailService;
//        }

//        #region Index

//        public IActionResult Index()
//        {
//            List<Order> orders = _datacontext.Orders.Include(x => x.OrderedProducts).OrderByDescending(x => x.CreatedAt).ToList();
//            return View(orders);
//        }

//        #endregion

//        #region Edit

//        public IActionResult Edit(int id)
//        {
//            Order order = _datacontext.Orders.Include(x => x.OrderedProducts).FirstOrDefault(x => x.Id == id);
//            if (order == null)
//            {
//                return RedirectToAction("index", "error", new { area = "" });
//            }

//            return View(order);
//        }

//        #endregion

//        #region Accept

//        public IActionResult Accept(int id)
//        {
//            Order order = _datacontext.Orders.Include(x => x.OrderedProducts).Include(x => x.User).FirstOrDefault(x => x.Id == id);
//            if (order == null)
//            {
//                return RedirectToAction("index", "error", new { area = "" });
//            }

//            order.Status = Database.Models.Enums.OrderStatus.Approved;

//            _datacontext.SaveChanges();

//            string body = string.Empty;
//            using (StreamReader reader = new StreamReader("wwwroot/templates/order.html"))
//            {
//                body = reader.ReadToEnd();
//            }

//            body = body.Replace("{{total}}", order.Total.ToString());
//            body = body.Replace("{{number}}", order.Id.ToString());
//            body = body.Replace("{{adress}}", order.Address.ToString());
//            body = body.Replace("{{date}}", order.CreatedAt.ToString("dd MMM yyyy HH:mm"));

//            string orderItems = string.Empty;

//            foreach (var item in order.OrderedProducts)
//            {
//                string tr = @$"<tr>
//                     <td width=\""75 %\"" align=\""left\"" style =\""font - family: Open Sans, Helvetica, Arial, sans-serif; font - size: 16px; font - weight: 400; line - height: 24px; padding: 15px 10px 5px 10px;\"" > {item.ProductName} </td>
//                     <td width=\""25 %\"" align=\""left\"" style =\""font - family: Open Sans, Helvetica, Arial, sans-serif; font - size: 16px; font - weight: 400; line - height: 24px; padding: 15px 10px 5px 10px;\"" > {item.Count} x ${item.SalePrice}.00 </td>
//                </tr>";

//                orderItems += tr;
//            }

//            body = body.Replace("{{total}}", order.Total.ToString()).Replace("{{orderItems}}", orderItems).Replace("{{number}}", order.Id.ToString())
//                       .Replace("{{adress}}", order.Address.ToString()).Replace("{{date}}", order.CreatedAt.ToString("dd MMM yyyy HH:mm"));

//            _emailService.Send(order.Email, "Order accepted", body);

//            return RedirectToAction("index");
//        }

//        #endregion

//        #region Reject

//        public IActionResult Reject(int id)
//        {
//            Order order = _datacontext.Orders.FirstOrDefault(x => x.Id == id);
//            if (order == null)
//            {
//                return RedirectToAction("index", "error", new { area = "" });
//            }

//            order.Status = Database.Models.Enums.OrderStatus.Rejected;

//            _datacontext.SaveChanges();

//            return RedirectToAction("index");
//        }

//        #endregion
//    }
//}