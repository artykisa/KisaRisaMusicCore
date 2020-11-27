using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KisaRisaMusicCore.Models;
using KisaMusic.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace KisaMusicCore.Controllers
{
    public class ApplicationUserController : Controller
    {
        // GET
        List<ApplicationUser> users = new List<ApplicationUser> 
        {
            new ApplicationUser { FirstName = "Artyom" },
            new ApplicationUser { FirstName = "Evgeniy"}
        };
        
        
        
        public IActionResult Index()
        {
            return View(users);
        }
        
        public IActionResult WorkPanel()
        {
            return View();
        }
        
        public ViewResult Login()
        {
            return View();
        }
        
        
        public ViewResult Logout()
        {
            return View();
        }
        
        public ViewResult Register()
        {
            return View();
        }
        
        public ViewResult Profile()
        {
            return View();
        }
        
        public IActionResult ChatPage()
        {
            return View();
        }
    }
}