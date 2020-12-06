using System.IO;
using System.Linq;
using System.Security.Claims;
using KisaRisaMusicCore.Models;
using KisaRisaMusicCore.Data;
using KisaRisaMusicCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KisaRisaMusicCore.Controllers
{
    public class TrackUserController : Controller
    {
        private ApplicationDbContext db;
        private ILogger _logger;
        public TrackUserController(ApplicationDbContext db, ILogger<FileLogger> logger)
        {
            _logger = logger;
            this.db = db;
        }
        // GET
        public IActionResult Index()
        {
            return RedirectToRoute("default", new { controller = "Playlist", action = "Liked"});
        }
        public IActionResult Delete(int id_track)
        {
            string id_user = HttpContext.User.Identity.Name;
            var dataToDelete = db.TrackUsers.FirstOrDefault(p => p.TrackId == id_track && p.UserId == id_user);
            if (dataToDelete != null)
            {
                db.TrackUsers.Remove(dataToDelete);
                db.SaveChanges();
                _logger.Log(LogLevel.Information, "Deleted track from user");
            }
            else
            {
                _logger.Log(LogLevel.Error, "Can't delete track from user");
            }

            return RedirectToRoute("default", new { controller = "Playlist", action = "Liked"});
        }
        
        public IActionResult Add(int id_track)
        {
            var id_user = HttpContext.User.Identity.Name;
            var dataToAdd = new TrackUser {TrackId = id_track,UserId = id_user};
            db.TrackUsers.Add(dataToAdd);
            db.SaveChanges();
            _logger.Log(LogLevel.Information, "Add track to user");
            return RedirectToRoute("default", new { controller = "Playlist", action = "Liked"});
        }
    }
}