using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Services.MtgApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/**
 * This controller will mostly be used to search for cards, and will be bridging the gap between our
 * user interface and the mtg api
 * 
 * V1 search options
 * Name
 * Colour
 * Power
 * Toughness
 * Converted Mana Cost (CMC)
 * Rarity
 * 
 * V2
 * Set
 * super type
 * Type
 * Subtype
 * Card text
 */
namespace LastLibrary.Controllers
{
    [Produces("application/json")]
    [Route("api/Card/{cardName}")]
    public class CardController : Controller
    {
        private IMtgApiService MtgApiService { get; }

        public CardController(IMtgApiService mtgApiService)
        {
            MtgApiService = mtgApiService;
        }

        /**
         * This will be the basic "Get cards by it's name" search
         */
        public CardsModel Get(string cardName)
        {
            //fire off asynchronous GET call
            var cardGetRequest = MtgApiService.SearchForCards(cardName);

            //wait for it to finish
            Task.WaitAny(cardGetRequest);

            //return the collection of cards
            return cardGetRequest.Result;
        }

        /**
         * This will be the more detailed search
         */
        public CardsModel Post(string cardName)
        {
            throw new NotImplementedException();
        }


    }
}