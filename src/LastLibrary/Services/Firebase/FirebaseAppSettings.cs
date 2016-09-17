using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Services.Firebase
{
    public class FirebaseAppSettings
    {
        public string ApiKey { get; set; }
        public string AuthDomain { get; set; }
        public string DatabaseURL { get; set; }
        public string StorageBucket { get; set; }
        public string MessagingSenderId { get; set; }
    }
}
