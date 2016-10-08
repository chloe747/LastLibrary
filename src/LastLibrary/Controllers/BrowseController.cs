using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using LastLibrary.Models.BrowseModels;
using LastLibrary.Models.DeckManagerViewModel;
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

        /**
         * Route used to view detailed information about a deck
         */
        public IActionResult Deck(string id)
        {
            //check to see if the deck exists
            DeckModel deck;
            try
            {
                deck = NoSqlService.GetDeckById(id);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            //check to see if the user is logged in
            if (User.Identity.IsAuthenticated)
            {
                var isUsersDeck = string.Compare(deck.Creator, User.Identity.Name, StringComparison.CurrentCulture) == 0;
                //if the user is logged in, they can view public decks and thier own decks
                if (!deck.IsPublic && !isUsersDeck)
                {
                    return RedirectToAction("Index");
                }
                //if this is the user's deck, they give them editing/deletion controls
                var viewModel = new DeckViewModel()
                {
                    Deck = deck,
                    DeckId = deck.Id.ToString(),
                    IsCreator = isUsersDeck
                };
                return View(viewModel);
            }
            else
            {
                //if the user is not logged in, they can only view public decks
                if (!deck.IsPublic)
                {
                    return RedirectToAction("Index");
                }
                var viewModel = new DeckViewModel()
                {
                    Deck = deck,
                    IsCreator = false
                };
                return View(viewModel);
            }
        }
    }
}