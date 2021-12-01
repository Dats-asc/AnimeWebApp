using System;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnimeWebApp.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _db;
        
        public AccountController(ApplicationDbContext context)
        {
            _db = context;
        }
        
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Registration(UserViewModel model)
        {
            User user =  _db.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null)
            {
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = model.Email,
                    Username = model.Username,
                    Password = model.Password
                };
                _db.Add(newUser);
                await _db.SaveChangesAsync();
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Email already registered");
            }
            return View(model);
        }
    }
}