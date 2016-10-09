using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.HomeModels;
using LastLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    public class HomeController : Controller
    {
        private INoSqlService NoSqlService { get; }

        public HomeController(INoSqlService noSqlService)
        {
            NoSqlService = noSqlService;
        }

        public IActionResult Index()
        {
            //grab the top rated decks
            var homeModel = new HomeViewModel()
            {
                TopRatedDecks = NoSqlService.GetDecksByTopRated(5)
            };
            return View(homeModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
