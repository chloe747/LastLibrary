using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Models.DeckManagerViewModel;
using LastLibrary.Services.Firebase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    [Produces("application/json")]
    [Route("api/Deck")]
    public class DeckController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private IFirebaseService FirebaseService { get; }

        public DeckController(UserManager<ApplicationUser> userManager, IFirebaseService firebaseService)
        {
            UserManager = userManager;
            FirebaseService = firebaseService;
        }

        /**
         * Route used to save a new deckModel
         */
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody] DeckModel deckModel)
        {
            //if the request has an invalid body
            if (!ModelState.IsValid)
            {
                return HttpStatusCode.BadRequest;
            }

            //otherwise, grab the current logged in user
            var user = await UserManager.GetUserAsync(HttpContext.User);

            //set the Creator to the current user
            deckModel.Creator = (user == null) ?  "Postman Test" : user.UserName;

            //set the creation time to now
            deckModel.CreationDate = DateTime.Now;

            //TODO: Implement how to send card model data from the front end to the back end

            //write the deck out to firebase
            return await FirebaseService.WriteToFirebase(deckModel);
        }
    }
}