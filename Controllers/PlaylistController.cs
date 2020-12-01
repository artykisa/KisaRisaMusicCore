using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KisaRisaMusicCore.Models;
using Microsoft.Extensions.Configuration;
using System.Web;
using KisaRisaMusicCore.Data;
using SQLitePCL;

namespace KisaMusicCore.Controllers
{
    public class PlaylistController : Controller
    {
        // GET
        private ApplicationDbContext db;
        public PlaylistController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Liked()
        { 
           // var list_tracks = db.Tracks.OrderBy(a => a.Id);
           //var list_tracks = db.Tracks.OrderBy(a => a.Id);
           var files = db.FileKisas.ToList();
           var artists = db.Artists.ToList();
           var albums = db.Albums.ToList();
           var userId = HttpContext.User.Identity.Name;

           var listOfTrackUserId = db.TrackUsers.Where(p=>p.UserId==userId).ToList();
           
           var listOfTrackId = new List<int>();
           for(int i = 0; i < listOfTrackUserId.Count; i++)
           {
               listOfTrackId.Add(listOfTrackUserId[i].TrackId);
           }
           var tracks = db.Tracks
               .Where(p=>listOfTrackId.Contains(p.Id))
                   .Include(t => t.FileKisa)
                   .ToList();

           return View(tracks);
        }
        
        public ViewResult AllTracks()
        { 
            // var list_tracks = db.Tracks.OrderBy(a => a.Id);
            //var list_tracks = db.Tracks.OrderBy(a => a.Id);
            var files = db.FileKisas.ToList();
            var artists = db.Artists.ToList();
            var albums = db.Albums.ToList();
           
            var tracks = db.Tracks
                .Include(t => t.FileKisa)
                .ToList();

            return View(tracks);
        }
        
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            string authData = $"Login: {login}   Password: {password}";
            return Content(authData);
        }
    }
}