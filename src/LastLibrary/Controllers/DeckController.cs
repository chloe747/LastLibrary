using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Models.DeckManagerViewModel;
using LastLibrary.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LastLibrary.Controllers
{
    [Produces("application/json")]
    public class DeckController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private INoSqlService NoSqlService { get; }

        public DeckController(UserManager<ApplicationUser> userManager, INoSqlService noSqlService)
        {
            UserManager = userManager;
            NoSqlService = noSqlService;
        }

        /**
         * Route used to save a new deckModel
         */
        [HttpPost]
        [Route("api/Deck")]
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
            return NoSqlService.WriteDeck(deckModel);
        }

        [HttpGet]
        [Route("api/Deck")]
        public async Task<ICollection<DeckModel>> GetDecksForUser()
        {
            //make sure the user is logged in
            //            if (!User.Identity.IsAuthenticated)
            //            {
            //                return HttpStatusCode.Forbidden;
            //            }

            //get the username
            var user = await UserManager.GetUserAsync(HttpContext.User);

            //set the Creator to the current user
            var userName = (user == null) ? "Postman Test" : user.UserName;
            
            //get the decks from mongoDb
            return NoSqlService.GetDecksForUser(userName);
        }

        [HttpGet]
        [Route("api/Deck/{deckName}")]
        public ICollection<DeckModel> GetDecksByDeckName(string deckName)
        {
            //get the decks from mongoDb
            return NoSqlService.GetDecksByDeckName(Uri.UnescapeDataString(deckName));
        }

        [HttpGet]
        [Route("api/Deck/{userName}/{deckName}")]
        public ICollection<DeckModel> GetDecksByUserNameAndDeckName(string userName, string deckName)
        {
            //get the decks from mongoDb
            return NoSqlService.GetDecksByUserNameAndDeckName(Uri.UnescapeDataString(userName), Uri.UnescapeDataString(deckName));
        }

    }
}