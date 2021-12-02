using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp.Models;
using AnimeWebApp.Views.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeWebApp.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _db;
        
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        
        public AccountController(ApplicationDbContext context, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _db = context;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }
        
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
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
                            UserProfile = new Profile()
                            {
                                Id = Guid.NewGuid()
                            }
                        };
                        _db.Users.Add(currentUser);
                        await _db.SaveChangesAsync();

                        var token = jWTAuthenticationManager.Authenticate(currentUser);
                        Response.Cookies.Append("token", token);
                        RedirectToAction("Index", "Home");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        ModelState.AddModelError("", $"Пользователь с такой почтой или именем пользователя  уже зарегистрирован.");
                }
            }
            else
                ModelState.AddModelError("", $"Не все поля заполнены.");
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["token"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == Encryption.EncryptString(model.Password));
            if (user == null)
            {
                return RedirectToAction("Registration", "Account");
            }
            var token = jWTAuthenticationManager.Authenticate(user);

            if (token == null)
                return RedirectToAction("Login", "Account");
            else
            {
                Response.Cookies.Append("token", token);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            var token = Request.Cookies["token"];
            if (token == null)
                return RedirectToAction("Login", "Account");
            Response.Cookies.Delete("token");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Profile()
        {
            if (Request.Cookies["token"] != null)
                return RedirectToAction("Index", "Home");
            else
            {
                var stream = Request.Cookies["token"];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
                var user = _db.Users.FirstOrDefault(u => u.Id.ToString() == id);
                var userProfile = user.UserProfile;;
                var model = new ProfileViewModel()
                {
                    Sex = userProfile.Sex,
                    City = userProfile.City,
                    Description = userProfile.Description
                };
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel model)
        {
            return Ok();
        }
    }
}