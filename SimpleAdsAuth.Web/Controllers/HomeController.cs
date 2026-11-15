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

        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult NewAd(SimpleAd simpleAd)
        {
            var repo = new SimpleAdRepository(_connectionString);
            simpleAd.UserId = int.Parse(User.Identity.Name);
            repo.AddAd(simpleAd);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult MyAccount()
        {
            var repo = new SimpleAdRepository(_connectionString);

            var viewModel = new AccountViewModel
            {
                SimpleAds = repo.GetAdsForUser(int.Parse(User.Identity.Name))
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult DeleteAd(int id)
        {
            var repo = new SimpleAdRepository(_connectionString);
            //can also get id for ad & check if matches instead of getting all ads for user
            if (repo.GetAdIdsForUser(int.Parse(User.Identity.Name)).Contains(id))
            {
                repo.DeleteAd(id);
            }

            return RedirectToAction("Index");
        }
    }
}
