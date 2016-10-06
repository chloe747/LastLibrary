using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        public HttpStatusCode Post([FromBody] DeckModel deckModel)
        {
            //if the request has an invalid body
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //make sure the user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //otherwise, grab the current logged in user
            var getUserTask = UserManager.GetUserAsync(HttpContext.User);

            Task.WaitAny(getUserTask);

            var user = getUserTask.Result;

            //set the Creator to the current user
            deckModel.Creator = user.UserName;

            //set the creation time to now
            deckModel.CreationDate = DateTime.Now;

            //write the deck out to firebase
            var response =  NoSqlService.WriteDeck(deckModel);
            
            //this is hackey, but if there is an error, throw an exception
            if (response != HttpStatusCode.OK)
            {
                throw new HttpResponseException(response);
            }

            return response;
        }

        [HttpGet]
        [Route("api/Deck")]
        public async Task<ICollection<DeckModel>> GetDecksForUser()
        {
            //make sure the user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                throw new HttpRequestException("No user logged in");
            }

            //get the user's data
            var user = await UserManager.GetUserAsync(HttpContext.User);
            
            //get the decks from mongoDb
            return NoSqlService.GetDecksForUser(user.UserName);
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