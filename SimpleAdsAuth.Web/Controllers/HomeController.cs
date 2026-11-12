using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAdsAuth.Data;
using SimpleAdsAuth.Web.Models;
using System.Diagnostics;

namespace SimpleAdsAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString
            = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;Trust Server Certificate=true;";
        public IActionResult Index()
        {
            var repo = new SimpleAdRepository(_connectionString);

            return View(new HomeViewModel
            {
                SimpleAds = repo.GetAllAds()
            });
        }

        public IActionResult NewAd()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public IActionResult NewAd(SimpleAd simpleAd)
        {
            var repo = new SimpleAdRepository(_connectionString);
            repo.AddAd(simpleAd);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new SimpleAdRepository(_connectionString);

            var viewModel = new AccountViewModel();
            var id = int.Parse(User.Identity.Name);
            viewModel.SimpleAds = repo.GetAdsForUser(id);
            if (viewModel.SimpleAds == null)
            {
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var repo = new SimpleAdRepository(_connectionString);
            if (repo.GetAdIdsForUser(int.Parse(User.Identity.Name)).Contains(id))
            {
                repo.DeleteAd(id);
            }
            
            return RedirectToAction("Index");
        }
    }
}
