using System;
using La_Grazia.Database;
using La_Grazia.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Controllers
{
    public class ContactController : Controller
    {
        private readonly DataContext _datacontext;

        public ContactController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        #region Index

        public IActionResult Index()
        {
            Setting setting = _datacontext.Settings.FirstOrDefault();
            return View(setting);
        }

        #endregion
    }
}