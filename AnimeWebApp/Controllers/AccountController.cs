using System;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp.Models;
using AnimeWebApp.Views.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ViewResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid && model.Password == model.ConfirmPassword)
            {
                if (ModelState.IsValid)
                {
                    User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email || u.Username == model.Username);
                    if (user == null)
                    {
                        var currentUser = new User
                        {
                            Id = Guid.NewGuid(),
                            Email = model.Email,
                            Password = Encryption.EncryptString(model.Password),
                            Username = model.Username,
                        };
                        _db.Users.Add(currentUser);
                        await _db.SaveChangesAsync();

                        //await Authentificate(currentUser);

                        //return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", $"Пользователь с почтой {model.Email} уже зарегистрирован.");
                }
            }
            else
                ModelState.AddModelError("", $"Не все поля заполнены.");
            return View(model);
        }
    }
}