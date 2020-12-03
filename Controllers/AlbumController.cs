using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace KisaRisaMusicCore.Controllers
{
    public class AlbumController : Controller
    {
        // GET
        private ApplicationDbContext db;
        private ILogger _logger;
        public AlbumController(ApplicationDbContext db, ILogger<FileLogger> logger, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.db = db;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult CRUD()
        {
            var listFile = db.Artists.ToList();
            var listAlbum = db.Albums.OrderBy(a => a.Id);
            return View(listAlbum);
        }
        public IActionResult Create() 
        {
            var list_files = db.FileKisas.ToList();
            PopulateDepartmentsDropDownList();
            _logger.Log(LogLevel.Information,"Album creating page");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Title,ArtistId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                _logger.Log(LogLevel.Information,"Album created");
                return RedirectToAction("CRUD");
            }
            else
            {
                _logger.Log(LogLevel.Error,"Cant load album to database: Id: "+album.Id +", Title: "+album.Title);
                return RedirectToAction("CRUD");
            }
        }
        public async Task<IActionResult> Details(int? id)
        {
            var listArtist = db.Artists.ToList();
            var listTra = db.Tracks.ToList();
            if (id != null)
            {
                Album album = db.Albums.
                    FirstOrDefault(p => p.Id == id);
                if (album != null)
                {
                    var listTracks = db.Tracks.Where(p => p.AlbumId == album.Id);
                    PopulateDepartmentsDropDownList(album.ArtistId);
                    _logger.Log(LogLevel.Information,"Album delails");
                    return View(album);
                }

                _logger.Log(LogLevel.Error,"Try album delails but 'album' == null");
            }
            _logger.Log(LogLevel.Error,"Try album delails but 'album' == null");
            return NotFound();
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            var listArtist = db.Artists.ToList();
            if(id!=null)
            {
                Album album = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Albums, p=>p.Id==id);
                if (album != null)
                {
                    PopulateDepartmentsDropDownList(album.ArtistId);
                    _logger.Log(LogLevel.Information,"Album page edit");
                    return View(album);
                }
                _logger.Log(LogLevel.Error,"Try album edit but 'album' == null");
            }
            _logger.Log(LogLevel.Error,"Try album edit but 'id' == null");
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(Album album)
        {
            db.Albums.Update(album);
            await db.SaveChangesAsync();
            _logger.Log(LogLevel.Information,"Album updated");
            return RedirectToAction("CRUD");
        }
        
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Album album = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Albums, p => p.Id == id);
                if (album != null)
                {
                    _logger.Log(LogLevel.Information,"Album confirm deleted");
                    return View(album);
                }

                _logger.Log(LogLevel.Error,"Try album confirm delete but 'album' == null");
            }
            _logger.Log(LogLevel.Error,"Try album confirm delete but 'id' == null");
            return NotFound();
        }
 
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Album album = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(db.Albums, p => p.Id == id);
                if (album != null)
                {
                    db.Albums.Remove(album);
                    await db.SaveChangesAsync();
                    _logger.Log(LogLevel.Information,"Album deleted");
                    return RedirectToAction("CRUD");
                }
            }
            _logger.Log(LogLevel.Error,"Try album edit but 'id' == null");
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