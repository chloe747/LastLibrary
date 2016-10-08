using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LastLibrary.Helpers;
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
        private DeckBuilderHelper DeckBuilderHelper { get; }

        public DeckController(UserManager<ApplicationUser> userManager, INoSqlService noSqlService)
        {
            UserManager = userManager;
            NoSqlService = noSqlService;
            DeckBuilderHelper = new DeckBuilderHelper();
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

            //work out the colour spread of the deck
            deckModel.ColourSpread = DeckBuilderHelper.CalculateColourSpread(deckModel.Cards);

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

        /**
         * Route used to update a deck
         */
        [HttpPut]
        [Route("api/Deck/{id}")]
        public HttpStatusCode Put(string id, [FromBody] DeckModel deckModel)
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

            //get the user        
            var getUserTask = UserManager.GetUserAsync(HttpContext.User);

            Task.WaitAny(getUserTask);

            var user = getUserTask.Result;

            //get the current deck with the requested id
            DeckModel deck;
            try
            {
                deck = NoSqlService.GetDeckById(id);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            //make sure the deck belongs to the current user
            if (string.Compare(deck.Creator, user.UserName, StringComparison.CurrentCulture) != 0)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //set the Creator to the current user
            deckModel.Creator = user.UserName;

            //work out the colour spread of the deck
            deckModel.ColourSpread = DeckBuilderHelper.CalculateColourSpread(deckModel.Cards);

            //set the creation time to now
            deckModel.CreationDate = deck.CreationDate;

            //copy the comments over
            deckModel.Comments = deck.Comments;

            //update the deck with the new data
            HttpStatusCode statusCode;
            try
            {
                statusCode = NoSqlService.UpdateDeckById(id, deckModel);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
            return statusCode;
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

        /**
         * Route to delete a deck by ID
         */
        [HttpDelete]
        [Route("api/Deck/{id}")]
        public HttpStatusCode DeleteDeckById(string id)
        {            
            //make sure the user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //get the user        
            var getUserTask = UserManager.GetUserAsync(HttpContext.User);

            Task.WaitAny(getUserTask);

            var user = getUserTask.Result;

            //get the deck to delete
            DeckModel deck;
            try
            {
                deck = NoSqlService.GetDeckById(id);
            }
            catch (HttpResponseException e)
            {
                throw e;
            }

            //make sure that the deck belongs to the user
            if (string.Compare(user.UserName, deck.Creator, StringComparison.CurrentCulture) != 0)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //delete the deck
            return NoSqlService.DeleteDeck(id);
        }

        [HttpPost]
        [Route("api/Deck/Comment/{deckId}")]
        public HttpStatusCode AddCommentToDeck(string deckId, [FromBody] CommentData comment)
        {
            //make sure the user is logged in to comment
            if (!User.Identity.IsAuthenticated)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //get the deck to delete
            DeckModel deck;
            try
            {
                deck = NoSqlService.GetDeckById(deckId);
            }
            catch (HttpResponseException e)
            {
                throw e;
            }

            //get the user        
            var getUserTask = UserManager.GetUserAsync(HttpContext.User);

            Task.WaitAny(getUserTask);

            var user = getUserTask.Result;           
             
            //make sure the deck is public, you can only comment on public decks if you don't own them
            if (!deck.IsPublic && string.Compare(deck.Creator, user.UserName, StringComparison.CurrentCulture) != 0)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            //add the username to the comment data
            comment.Commenter = user.UserName;

            //add the current date to the comment
            comment.CommentDate = DateTime.Now;
            
            //save the comment
            HttpStatusCode result;
            try
            {
                result = NoSqlService.AddCommentToDeck(comment, deckId);
            }
            catch (HttpResponseException e)
            {
                throw e;
            }

            return result;
        }


    }
}