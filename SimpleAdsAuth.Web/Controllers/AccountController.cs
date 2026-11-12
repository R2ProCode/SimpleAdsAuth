using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleAdsAuth.Data;
using SimpleAdsAuth.Web.Models;
using System.Security.Claims;

namespace SimpleAdsAuth.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString
            = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;Trust Server Certificate=true;";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(user, password);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(email, password);
            if (user == null)
            {
                TempData["Message"] = "Invalid Login";
                return RedirectToAction("Login");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "SimpleAdsAuth", ClaimTypes.NameIdentifier, "roles")
            )).Wait();

            return RedirectToAction("MyAccount", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }       
    }
}
