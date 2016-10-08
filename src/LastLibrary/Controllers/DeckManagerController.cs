using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;
using LastLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace LastLibrary.Controllers
{
    public class DeckManagerController : Controller
    {
        private INoSqlService NoSqlService { get; }

        public DeckManagerController(INoSqlService noSqlService)
        {
            NoSqlService = noSqlService;
        }

        // GET: DeckManager
        public ActionResult Index()
        {
            bool isLoggedIn = User.Identity.IsAuthenticated;
            if (isLoggedIn)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Edit(string id)
        {
            //verify that the user is logged in
            bool isLoggedIn = User.Identity.IsAuthenticated;
            if (isLoggedIn)
            {
                //now, verify that the deck exists
                DeckModel deckToEdit;
                try
                {
                    deckToEdit = NoSqlService.GetDeckById(id);
                }
                catch
                {
                    return RedirectToAction("Index", "DeckManager");
                }

                //if the deck exists, check to see if the logged in user owns the deck
                if (String.Compare(User.Identity.Name, deckToEdit.Creator, StringComparison.CurrentCulture) != 0)
                {
                    return RedirectToAction("Index", "DeckManager");
                }

                //since the user owns this deck, and the deck was found, let them edit the deck
                return View(deckToEdit);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}