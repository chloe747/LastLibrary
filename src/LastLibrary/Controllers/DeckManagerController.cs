using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using LastLibrary.Models.DeckManagerViewModel;
using LastLibrary.Services.Firebase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace LastLibrary.Controllers
{
    public class DeckManagerController : Controller
    {

        private IFirebaseService FirebaseService { get; }

        public DeckManagerController(IFirebaseService firebaseService)
        {
            FirebaseService = firebaseService;
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

        // GET: DeckManager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DeckManager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeckManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(DeckModel deckModel)
        {
            try
            {
                var response = await FirebaseService.WriteToFirebase(deckModel);

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: DeckManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DeckManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DeckManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DeckManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}