using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LoginRegisterCSharpCoDo.Models;

namespace LoginRegisterCSharpCoDo.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get;set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }
        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(dbUser => dbUser.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View("Index");
                }
                else
                {
                    _context.Users.Add(newUser);
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    _context.SaveChanges();
                    return Redirect("/signin");
                }
            }
            else
            {
                return View("Index");
            }
        }
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View("SignIn");
        }
        [HttpPost("login")]
        public IActionResult Login(LoginUser userLogInfo)
        {
            if(ModelState.IsValid)
            {
                var UserInDb = _context.Users.FirstOrDefault(user => user.Email == userLogInfo.Email);
                if(UserInDb == null)
                {
                    ModelState.AddModelError("Email", "Email is not registered. Please use a different email address or register.");
                    return View("SignIn");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userLogInfo, UserInDb.Password, userLogInfo.Password);
                    if(result ==0)
                    {
                        ModelState.AddModelError("Password", "Password is incorrect.");
                        return View("SignIn");
                    }
                    else
                    {
                        User userLoggedIn = _context.Users.FirstOrDefault(user => user.Email == userLogInfo.Email);
                        HttpContext.Session.SetInt32("UserId", userLoggedIn.UserId);
                        return Redirect("/success");
                    }
                }
            }
            else
            {
                return View("SignIn");
            }
        }
        [HttpGet("success")]
        public IActionResult Success()
        {
            int? UserInSession = HttpContext.Session.GetInt32("UserId");
            if(UserInSession == null)
            {
                return Redirect("/");
            }
            else
            {
                User LoggedInUser = _context.Users.FirstOrDefault(user => user.UserId == UserInSession);
                return View("Success", LoggedInUser);
            }
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
