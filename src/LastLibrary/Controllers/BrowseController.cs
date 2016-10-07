using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.BrowseModels;
using LastLibrary.Services;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    public class BrowseController : Controller
    {
        private INoSqlService NoSqlService { get; }

        public BrowseController(INoSqlService noSqlService)
        {
            NoSqlService = noSqlService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //if the user is logged in, get the user's decks
        public IActionResult Decks()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated;
            if (isLoggedIn)
            {
                //get the user's decks from mongoDB
                var usersDecks = new MyDecksModel()
                {
                    UsersDecks = NoSqlService.GetDecksForUser(User.Identity.Name)
                };
                return View(usersDecks);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}