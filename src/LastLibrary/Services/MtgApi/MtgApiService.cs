using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LastLibrary.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace LastLibrary.Services.MtgApi
{
    public class MtgApiService : IMtgApiService
    {
        private string CardsUrl { get; }

        public MtgApiService(IOptions<MtgApiSettings> settings)
        {
            CardsUrl = settings.Value.Urls.Cards;
        }

        public async Task<CardsModel> SearchForCardsByName(string cardName)
        {
            // create the get request url
            var requestUrl = CardsUrl + "?name=" + cardName;

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
    }
}
