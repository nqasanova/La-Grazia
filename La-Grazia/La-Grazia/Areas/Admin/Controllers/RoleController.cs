using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database.Models;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (!ModelState.IsValid) return View();

            await _roleManager.CreateAsync(identityRole);
            await _roleManager.UpdateAsync(identityRole);

            return RedirectToAction("index");
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole deleteRole = _roleManager.Roles.FirstOrDefault(x => x.Id == id);

            await _roleManager.DeleteAsync(deleteRole);
            await _roleManager.UpdateAsync(deleteRole);

            return RedirectToAction("index");
        }

        #endregion
    }
}