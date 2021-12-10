using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AnimeWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        public HomeController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var currentUser = HttpContext.User;
            
            if (Request.Cookies["token"] == null || Request.Cookies["token"] == "")
            {
                return View();
            }
            var stream = Request.Cookies["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;

            var user = _db.Users.FirstOrDefault(u => u.Id.ToString() == id);

            return View(user);
        }
        
        public IActionResult MainPage()
        {
            return View();
        }
    }
}