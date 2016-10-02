using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Models.ConfigurationModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LastLibrary.Services.MtgApi
{
    public class MtgApiService : IMtgApiService
    {
        private string CardsUrl { get; }

        public MtgApiService(IOptions<MtgApiConfiguration> settings)
        {
            CardsUrl = settings.Value.Urls.Cards;
        }

        public CardsModel SearchForCards(string cardName)
        {
            // create the get request url
            var requestUrlComponent = "?name=" + cardName;

            //create the async task
            var requestTask = RequestCards(requestUrlComponent);

            //wait for it to finish
            Task.WaitAny(requestTask);

            return requestTask.Result;
        }

        public CardsModel SearchForCards(string cardName, CardSearchOptionsModel cardOpts)
        {
            //create the params for the request url
            ICollection<string> queryParams = new Collection<string>();

            //parse and add the name options
            if (cardName != null)
            {
                queryParams.Add("name=" + cardName);
            }

            //parse and add the colour options
            var colourOpts = ParseColourParams(cardOpts);
            if (colourOpts != null)
            {
                queryParams.Add(colourOpts);
            }

            //parse and add the Power options
            if (cardOpts.Power != null)
            {
                var powerValue = cardOpts.Power;
                if (cardOpts.PowerOperator != null)
                {
                    powerValue = cardOpts.PowerOperator + powerValue;
                }
                queryParams.Add("power=" + powerValue);
            }

            //parse and add the Toughness options
            if (cardOpts.Toughness != null)
            {
                var toughnessValue = cardOpts.Toughness;
                if (cardOpts.ToughnessOperator != null)
                {
                    toughnessValue = cardOpts.ToughnessOperator + toughnessValue;
                }
                queryParams.Add("toughness=" + toughnessValue);
            }

            //parse and add the CMC options
            if (cardOpts.Cmc != null)
            {
                var cmcValue = cardOpts.Cmc;
                if (cardOpts.CmcOperator != null)
                {
                    cmcValue = cardOpts.CmcOperator + cmcValue;
                }
                queryParams.Add("cmc=" + cmcValue);
            }

            //parse and add the Rarity options
            if (cardOpts.Rarity != null)
            {
                queryParams.Add("rarity=" + Uri.EscapeUriString(cardOpts.Rarity));
            }

            //create the request params
            var requestUrlComponent = "";
            foreach (var queryParam in queryParams)
            {
                if (requestUrlComponent == "")
                {
                    requestUrlComponent = "?" + queryParam;
                }
                else
                {
                    requestUrlComponent += "&" + queryParam;
                }
            }

            //create the async task
            var requestTask = RequestCards(requestUrlComponent);

            //wait for it to finish
            Task.WaitAny(requestTask);

            return requestTask.Result;
        }

        private async Task<CardsModel> RequestCards(string uriParams)
        {            
            //if params exist, append to cards url
            var requestUrl = CardsUrl + uriParams;

            //create the GET request
            var client = new HttpClient();
            var response = await client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            //get the response from the request
            var responseBody = await response.Content.ReadAsStringAsync();

            //deserialise the request
            var result = JsonConvert.DeserializeObject<CardsModel>(responseBody);
            return result;
        }

        private string ParseColourParams(CardSearchOptionsModel cardOpts)
        {
            //&colors=red,white,blue
            ICollection<string> selectedColours = new Collection<string>();
            if (cardOpts.IsBlue)
            {
                selectedColours.Add("blue");
            }
            if (cardOpts.IsRed)
            {
                selectedColours.Add("red");
            }
            if (cardOpts.IsBlack)
            {
                selectedColours.Add("black");
            }
            if (cardOpts.IsWhite)
            {
                selectedColours.Add("white");
            }
            if (cardOpts.IsGreen)
            {
                selectedColours.Add("green");
            }
            if (cardOpts.IsColorless)
            {
                selectedColours.Add("colorless");
            }

            if (selectedColours.Any())
            {
                var colourRequest = "";
                foreach (var colour in selectedColours)
                {
                    if (colourRequest == "")
                    {
                        colourRequest += colour;
                    }
                    else
                    {
                        colourRequest += "," + colour;
                    }
                }
                return "colors=" + colourRequest;
            }
            return null;
        }
    }
}
