using System;
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
            return View();
        }
        
        public IActionResult MainPage()
        {
            return View();
        }
    }
}