using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KisaRisaMusicCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace KisaRisaMusicCore.Controllers
{
    public class FileController : Controller
    {
        public FileController(ApplicationDbContext _db)
        {
            db = _db;
        }
        private ApplicationDbContext db;
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="admin")]
        public IActionResult CRUD()
        {
            if (!db.FileKisas.Any())
            {
                var Files = new FileKisa[]
                {
                    new FileKisa{Id= 1, Name = "Test_Track_1", Path = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-1.mp3"},
                    new FileKisa{Id = 2, Name = "RandomTrack2", Path = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-2.mp3"}
                };
                foreach (var VARIABLE in Files)
                {
                    db.FileKisas.Add(VARIABLE);
                }
                db.SaveChanges();
            }

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
        public async Task<IActionResult> Create(FileKisa file)
        {
            db.FileKisas.Add(file);
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
                    db.FileKisas.Remove(file);
                    await db.SaveChangesAsync();
                    return RedirectToAction("CRUD");
                }
            }
            return NotFound();
        }
    }
    
}