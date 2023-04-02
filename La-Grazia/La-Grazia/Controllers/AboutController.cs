using System;
using La_Grazia.ViewModels;
using La_Grazia.Database;
using Microsoft.AspNetCore.Mvc;

namespace La_Grazia.Areas.Controllers
{
    public class AboutController : Controller
    {
        private readonly DataContext _datacontext;

        public AboutController(DataContext datacontext)
        {
            _datacontext = datacontext;
        }

        #region Index

        public IActionResult Index()
        {
            AboutViewModel aboutVM = new AboutViewModel
            {
                Abouts = _datacontext.Abouts.FirstOrDefault(),
                Features = _datacontext.Features.ToList(),
                Teams = _datacontext.Teams.ToList(),
                Settings = _datacontext.Settings.FirstOrDefault()
            };

            return View(aboutVM);
        }

        #endregion
    }
}