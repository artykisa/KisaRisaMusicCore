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
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KisaMusicCore.Controllers
{
    public class ApplicationUserController : Controller
    {
        // GET
        private ApplicationDbContext db;
        private ILogger _logger;
        private UserManager<IdentityUser> _userManager;
        public ApplicationUserController(ApplicationDbContext context,ILoggerFactory loggerFactory, UserManager<IdentityUser> userManager)
        {
            db = context;
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("AlbumController");
            _logger = logger;
            _userManager = userManager;
        }

        
        
        
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize(Roles="admin")]
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