using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace LastLibrary.Services.Firebase
{
    public class FirebaseService : IFirebaseService
    {
        private IFirebaseClient FirebaseClient { get; set; }

        public FirebaseService(FirebaseAppSettings fireBaseConfig)
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = fireBaseConfig.ApiKey,
                BasePath = fireBaseConfig.AuthDomain
            };
            FirebaseClient = new FirebaseClient(config);
        }

        public Task WriteToFirebase()
        {
            return Task.FromResult(0);
        }
    }
}
