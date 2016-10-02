using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models.ConfigurationModels;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LastLibrary.Services.MongoDb
{
    public class MongoDbService : INoSqlService
    {
        private string ConnectionUrl { get; }
        private MongoClient Client { get; }
        private IMongoDatabase Database { get; }
        private IMongoCollection<DeckModel> DecksCollection { get; }

        public MongoDbService(IOptions<MongoDbConfigurationModel> config)
        {
            //construct the connection URL
            ConnectionUrl = "mongodb://" + config.Value.UserName + ":" + config.Value.Password + config.Value.Url;

            //connect to mongoDb
            Client = new MongoClient(ConnectionUrl);

            //connect to the database
            Database = Client.GetDatabase("lastlibrary");

            //get the Decks collection
            DecksCollection = Database.GetCollection<DeckModel>("Decks");
        }

        public HttpStatusCode WriteDeck(DeckModel deckModel)
        {
            var result = DecksCollection.InsertOneAsync(deckModel);

            Task.WaitAny(result);
            return (result.IsFaulted != true && result.IsFaulted != true) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        }
    }
}
