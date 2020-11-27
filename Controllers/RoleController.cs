using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using KisaMusic.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;
using IdentityRole = Microsoft.AspNet.Identity.EntityFramework.IdentityRole;


namespace KisaRisaMusicCore.Controllers
{
    public class RoleController : Controller
    {
        // GET
        private Microsoft.AspNet.Identity.RoleManager<IdentityRole> roleManager;
        private Microsoft.AspNet.Identity.UserManager<ApplicationUser> userManager;
 
        public RoleController(Microsoft.AspNet.Identity.RoleManager<IdentityRole> roleMgr, Microsoft.AspNet.Identity.UserManager<ApplicationUser> userMrg)
        {
            roleManager = roleMgr;
            userManager = userMrg;
        }
 
        // other methods
 
        public ViewResult Index() => View(roleManager.Roles);
 
        private void Errors(IdentityResult result)
        {
            
        }
        public IActionResult Create() => View();
 
        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }
        public async Task<IActionResult> Update(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();
            foreach (ApplicationUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user.ToString(), role.Name) ? members : nonMembers;
                list.Add(user);
            }
            return View(new RoleEdit
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }
 
        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.AddIds ?? new string[] { })
                {
                    ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);
                    if (applicationUser != null)
                    {
                        result = await userManager.AddToRoleAsync(applicationUser.ToString(), model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
                foreach (string userId in model.DeleteIds ?? new string[] { })
                {
                    ApplicationUser applicationUser = await userManager.FindByIdAsync(userId);
                    if (applicationUser != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(applicationUser.ToString(), model.RoleName);
                        if (!result.Succeeded)
                            Errors(result);
                    }
                }
            }
 
            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));
            else
                return await Update(model.RoleId);
        }
    
    }
}