using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KisaRisaMusicCore.Models;
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
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize(Roles="admin")]
        public IActionResult WorkPanel()
        {
            return View();
        }
        public IActionResult ChatPage()
        {
            return View();
        }
    }
}