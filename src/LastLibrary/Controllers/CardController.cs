using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Services.MtgApi;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public CardsModel Get(string cardName)
        {
            //fire off asynchronous GET call
            var cardGetRequest = MtgApiService.SearchForCardsByName(cardName);

            //wait for it to finish
            Task.WaitAny(cardGetRequest);

            //return the collection of cards
            return cardGetRequest.Result;
        }


    }
}