using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KisaMusic.Domain.Models;
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KisaMusicCore.Controllers
{
    public class TrackController : Controller
    {
        // GET
        private ApplicationDbContext db;
        private ILogger _logger;
        public TrackController(ApplicationDbContext db, ILoggerFactory loggerFactory )
        {
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            var logger = loggerFactory.CreateLogger("TrackController");
            _logger = logger;
            this.db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult CRUD()
        {
            var list_file = db.FileKisas.ToList();
            var list_tracks = db.Tracks.OrderBy(a => a.Id);
            return View(list_tracks);
        }
        
        
        public IActionResult Create() 
        {
            var list_files = db.FileKisas.ToList();
            PopulateDepartmentsDropDownList();
            _logger.Log(LogLevel.Information,"Track created");
            return View();
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            var list_files = db.FileKisas.ToList();
            if(id!=null)
            {
                Track track = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Tracks, p=>p.Id==id);
                if (track != null)
                {
                    PopulateDepartmentsDropDownList(track.FileKisaId, track.ArtistId,track.AlbumId);
                    _logger.Log(LogLevel.Information,"Track edited");
                    return View(track);
                }
                _logger.Log(LogLevel.Error,"Try track edit but 'track' == null");
            }
            _logger.Log(LogLevel.Error,"Try track edit but 'id' == null");
            return NotFound();
        }
        
        /*[HttpPost]
        public async Task<IActionResult> Create(Track track)
        {
            var list_files = db.FileKisas.ToList();
            db.Tracks.Add(track);
            await db.SaveChangesAsync();
            return RedirectToAction("CRUD");
        }*/
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,FileKisaId, ArtistId, AlbumId")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Tracks.Add(track);
                await db.SaveChangesAsync();
                return RedirectToAction("CRUD");
            }
            else
            {
                _logger.Log(LogLevel.Error,"Cant load track to database: Id: "+track.Id +", Title: "+track.Title);
                return RedirectToAction("CRUD");
            }
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            var list_files = db.FileKisas.ToList();
            if (id != null)
            {
                Track track = db.Tracks.
                    FirstOrDefault(p => p.Id == id);
                if (track != null)
                {
                    PopulateDepartmentsDropDownList(track.FileKisaId, track.ArtistId,track.AlbumId);
                    _logger.Log(LogLevel.Information,"Track delails");
                    return View(track);
                }

                _logger.Log(LogLevel.Error,"Try track delails but 'track' == null");
            }
            _logger.Log(LogLevel.Error,"Try track delails but 'id' == null");
            return NotFound();
        }
        

        [HttpPost]
        public async Task<IActionResult> Edit(Track track)
        {
            db.Tracks.Update(track);
            await db.SaveChangesAsync();
            _logger.Log(LogLevel.Information,"Track updated");
            return RedirectToAction("CRUD");
        }
        
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Track track = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Tracks, p => p.Id == id);
                if (track != null)
                {
                    _logger.Log(LogLevel.Information,"Track confirm deleted");
                    return View(track);
                }

                _logger.Log(LogLevel.Error,"Try track confirm delete but 'track' == null");
            }
            _logger.Log(LogLevel.Error,"Try track confirm delete but 'id' == null");
            return NotFound();
        }
 
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Track track = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Tracks, p => p.Id == id);
                if (track != null)
                {
                    db.Tracks.Remove(track);
                    await db.SaveChangesAsync();
                    _logger.Log(LogLevel.Information,"Track deleted");
                    return RedirectToAction("CRUD");
                }
            }
            _logger.Log(LogLevel.Error,"Try track edit but 'id' == null");
            return NotFound();
        }
        
        private void PopulateDepartmentsDropDownList(object selectedDepartment = null, object selectedArtist = null, object selectedAlbum = null)
        {
            var filesQuery = db.FileKisas.ToList();
            ViewBag.FileKisa = new SelectList(filesQuery, "Id", "Name", selectedDepartment);
            var artistQuery = db.Artists.ToList();
            ViewBag.Artist = new SelectList(artistQuery, "Id", "Nickname", selectedArtist);
            var albumQuery = db.Albums.ToList();
            ViewBag.Album = new SelectList(albumQuery, "Id", "Title", selectedAlbum);
        }
    }
}