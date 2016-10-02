using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Models.DeckManagerViewModel;
using LastLibrary.Services.Firebase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    [Produces("application/json")]
    [Route("api/Deck")]
    public class DeckController : Controller
    {
        private IFirebaseService FirebaseService { get; }

        public DeckController(IFirebaseService firebaseService)
        {
            FirebaseService = firebaseService;
        }

        /**
         * Route used to save a new deck
         */
        [HttpPost]
        public HttpResponse Post([FromBody] Deck deck)
        {
            throw new NotImplementedException();
        }
    }
}