using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace LastLibrary.Controllers
{
    public class DeckManagerController : Controller
    {

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
            return RedirectToAction("Index", "Home");
        }
    }
}