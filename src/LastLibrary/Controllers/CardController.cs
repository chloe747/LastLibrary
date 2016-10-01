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
        [HttpGet]
        [Route("api/Card/{cardName}")]
        public CardsModel Get(string cardName)
        {
            //fire off the async GET call and return the result
            return MtgApiService.SearchForCards(cardName);
        }

        /**
         * This will be the more detailed search
         */
        [HttpPost]
        [Route("api/Card/{cardName}")]
        [Route("api/Card/")]
        public CardsModel Post(string cardName, [FromBody] CardSearchOptionsModel opts)
        {
            //fire off the async call and return the result
            return MtgApiService.SearchForCards(cardName, opts);
        }


    }
}