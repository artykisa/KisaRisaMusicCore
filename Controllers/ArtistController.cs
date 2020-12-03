using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KisaRisaMusicCore.Controllers
{
    public class ArtistController : Controller
    {
        private ApplicationDbContext db;
        private ILogger _logger;

        public ArtistController(ApplicationDbContext db, ILogger<FileLogger> logger)
        {
            _logger = logger;
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [Authorize(Roles="admin")]
        public IActionResult CRUD()
        {
            var listAlbum = db.Albums.ToList();
            var listArtist = db.Artists.OrderBy(a => a.Id);
            return View(listArtist);
        }
        [Authorize(Roles="admin")]
        public IActionResult Create() 
        {
            var listAlbum = db.Albums.ToList();
            PopulateDepartmentsDropDownList();
            _logger.Log(LogLevel.Information,"Artist creating page");
            return View();
        }
        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Create([Bind("Id,Nickname,ArtistId")] Artist artist)
        {
            db.Artists.Add(artist);
            await db.SaveChangesAsync();
            _logger.Log(LogLevel.Information,"Artist created");
            return RedirectToAction("CRUD");
            
        }
        public async Task<IActionResult> Details(int? id)
        {
            var listArtist = db.Artists.ToList();
            if (id != null)
            {
                Artist artist = db.Artists.
                    FirstOrDefault(p => p.Id == id);
                if (artist != null)
                {
                    var listAlbums = db.Albums.Where(p => p.Id == artist.Id).ToList();
                    artist.Albums = listAlbums;
                    PopulateDepartmentsDropDownList();
                    _logger.Log(LogLevel.Information,"Artist delails");
                    return View(artist);
                }

                _logger.Log(LogLevel.Error,"Try album delails but 'album' == null");
            }
            _logger.Log(LogLevel.Error,"Try album delails but 'album' == null");
            return NotFound();
        }
        
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var listArtist = db.Artists.ToList();
            if(id!=null)
            {
               Artist artist = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Artists, p=>p.Id==id);
                if (artist != null)
                {
                    PopulateDepartmentsDropDownList();
                    _logger.Log(LogLevel.Information,"Artist page edit");
                    return View(artist);
                }
                _logger.Log(LogLevel.Error,"Try artist edit but 'artist' == null");
            }
            _logger.Log(LogLevel.Error,"Try artist edit but 'id' == null");
            return NotFound();
        }
        
        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Edit(Artist artist)
        {
            db.Artists.Update(artist);
            await db.SaveChangesAsync();
            _logger.Log(LogLevel.Information,"Artist updated");
            return RedirectToAction("CRUD");
        }
        
        [HttpGet]
        [ActionName("Delete")]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Artist artist = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Artists, p => p.Id == id);
                if (artist != null)
                {
                    _logger.Log(LogLevel.Information,"Artist confirm deleted");
                    return View(artist);
                }

                _logger.Log(LogLevel.Error,"Try artist confirm delete but 'artist' == null");
            }
            _logger.Log(LogLevel.Error,"Try artist confirm delete but 'id' == null");
            return NotFound();
        }
 
        [HttpPost]
        [Authorize(Roles="admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Artist artist = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Artists, p => p.Id == id);
                if (artist != null)
                {
                    db.Artists.Remove(artist);
                    await db.SaveChangesAsync();
                    _logger.Log(LogLevel.Information,"Artist deleted");
                    return RedirectToAction("CRUD");
                }
            }
            _logger.Log(LogLevel.Error,"Try artist edit but 'id' == null");
            return NotFound();
        }
        
        
        private void PopulateDepartmentsDropDownList(object selectedArtist = null, object selectedAlbum = null)
        {
            var artistQuery = db.Artists.ToList();
            ViewBag.Artist = new SelectList(artistQuery, "Id", "Nickname", selectedArtist);
            var albumQuery = db.Albums.ToList();
            ViewBag.Album = new SelectList(albumQuery, "Id", "Title", selectedAlbum);
        }
        
    }
}