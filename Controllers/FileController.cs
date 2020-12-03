using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace KisaRisaMusicCore.Controllers
{
    public class FileController : Controller
    {
        public FileController(ApplicationDbContext _db, IWebHostEnvironment _appEnvironment,ILogger<FileLogger> logger)
        {
            db = _db;
            appEnvironment = _appEnvironment;
            _logger = logger;
        }
        private ApplicationDbContext db;
        private IWebHostEnvironment appEnvironment;
        private ILogger _logger;
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="admin")]
        public IActionResult CRUD()
        {
            var list_file = db.FileKisas.OrderBy(a => a.Id);
            return View(list_file);
        }
        [Authorize(Roles="admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id!=null)
            {
                FileKisa file = await db.FileKisas.FirstOrDefaultAsync(p=>p.Id==id);
                if (file != null)
                    return View(file);
            }
            return NotFound();
        }
        
        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Create(IFormFile uploadedFile)
        {
            if (uploadedFile.ContentType != "audio/mpeg")
            {
                _logger.Log(LogLevel.Error,"Wrong format!"+ uploadedFile.ContentType);
                return RedirectToAction("CRUD");
            }
            _logger.Log(LogLevel.Information,"Format file =" + uploadedFile.ContentType);
            string path = "/tracks/" + uploadedFile.FileName;
            string fullPath = appEnvironment.WebRootPath + path;
            if (!System.IO.File.Exists(fullPath))
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }

            FileKisa file2 = new FileKisa() { Name = uploadedFile.FileName, Path = path };
            db.FileKisas.Add(file2);
            await db.SaveChangesAsync();
            return RedirectToAction("CRUD");
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                FileKisa file = await db.FileKisas.FirstOrDefaultAsync(p => p.Id == id);
                if (file != null)
                    return View(file);
            }
            return NotFound();
        }
        

        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Edit(FileKisa file)
        {
            db.FileKisas.Update(file);
            await db.SaveChangesAsync();
            return RedirectToAction("CRUD");
        }
        
        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                FileKisa file = await db.FileKisas.FirstOrDefaultAsync(p => p.Id == id);
                if (file != null)
                    return View(file);
            }
            return NotFound();
        }
 
        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                FileKisa file = await db.FileKisas.FirstOrDefaultAsync(p => p.Id == id);
                if (file != null)
                {
                    string path = "/tracks/" + file.Name;
                    string fullPath = appEnvironment.WebRootPath + path;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    db.FileKisas.Remove(file);
                    await db.SaveChangesAsync();
                    return RedirectToAction("CRUD");
                }
            }
            return NotFound();
        }
    }
    
}