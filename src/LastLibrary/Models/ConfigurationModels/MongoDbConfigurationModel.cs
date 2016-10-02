using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Models.ConfigurationModels
{
    public class MongoDbConfigurationModel
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
