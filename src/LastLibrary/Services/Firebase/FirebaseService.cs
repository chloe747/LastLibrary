using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using LastLibrary.Models;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.Extensions.Options;

namespace LastLibrary.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private IFirebaseClient FirebaseClient { get; set; }

        public FirebaseService(IOptions<FirebaseAppSettingsModel> settings)
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = settings.Value.Secret,
                BasePath = settings.Value.DatabaseUrl
            };
            FirebaseClient = new FirebaseClient(config);
        }

        public async Task<HttpStatusCode> WriteToFirebase(Deck deck)
        {
            PushResponse response = await FirebaseClient.PushAsync("decks/set", deck);

            return response.StatusCode;
        }
    }
}
