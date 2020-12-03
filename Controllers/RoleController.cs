using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KisaMusic.Domain.Models;
using KisaRisaMusicCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace KisaRisaMusicCore.Controllers
{
    [Authorize(Roles="admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<IdentityUser> _userManager;
        private IEmailSender _emailSender;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;

        }
        public IActionResult Index() => View(_roleManager.Roles.ToList());
 
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
         
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
 
        public IActionResult UserList() => View(_userManager.Users.ToList());
 
        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
 
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // получаем пользователя
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);
 
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                string addedRolesString="";
                foreach (var role in addedRoles)
                {
                    addedRolesString += " "+role;
                }
                string removedRolesString="";
                foreach (var role in removedRoles)
                {
                    removedRolesString += " "+role;
                }
                _emailSender.SendEmailAsync(user.Email.ToString(), "Role changes",
                    "We have changed your roles." +
                    " Added:" + addedRolesString +
                    ". Removed:" + removedRolesString);
                
                return RedirectToAction("UserList");
            }
 
            return NotFound();
        }
    }
}