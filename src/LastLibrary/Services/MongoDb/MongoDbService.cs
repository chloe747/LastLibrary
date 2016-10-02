using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models.ConfigurationModels;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LastLibrary.Services.MongoDb
{
    public class MongoDbService : INoSqlService
    {
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

        private string ConnectionUrl { get; }
        private MongoClient Client { get; }
        private IMongoDatabase Database { get; }
        private IMongoCollection<DeckModel> DecksCollection { get; }

        public HttpStatusCode WriteDeck(DeckModel deckModel)
        {
            var result = DecksCollection.InsertOneAsync(deckModel);

            Task.WaitAny(result);
            return (result.IsFaulted != true) && (result.IsFaulted != true)
                ? HttpStatusCode.OK
                : HttpStatusCode.InternalServerError;
        }


        public ICollection<DeckModel> GetDecksForUser(string userName)
        {
            //construct a filter
            var filter = Builders<DeckModel>.Filter.Regex(u => u.Creator, new BsonRegularExpression("/^" + userName + "$/i"));

            //query mongoDb for user's decks
            var decksRequest =
                DecksCollection.Find(filter)
                    .ToListAsync();

            //wait for the task to finish
            Task.WaitAny(decksRequest);

            //return the result
            return decksRequest.Result;
        }

        public ICollection<DeckModel> GetDecksByDeckName(string deckName)
        {
            //construct a filter
            var filter = Builders<DeckModel>.Filter.Regex(u => u.DeckName, new BsonRegularExpression("/^" + deckName + "$/i"));

            //query mongoDb for user's decks
            var decksRequest =
                DecksCollection.Find(filter)
                    .ToListAsync();

            //wait for the task to finish
            Task.WaitAny(decksRequest);

            //return the result
            return decksRequest.Result;
        }

        public ICollection<DeckModel> GetDecksByUserNameAndDeckName(string userName, string deckName)
        {
            //construct the filter
            var filter = Builders<DeckModel>.Filter.And(
                                 Builders<DeckModel>.Filter.Regex(u => u.DeckName, new BsonRegularExpression("/^" + deckName + "$/i")),
                                 Builders<DeckModel>.Filter.Regex(u => u.Creator, new BsonRegularExpression("/^" + userName + "$/i")));

            //query mongoDb for user's decks
            var decksRequest =
                DecksCollection.Find(filter)
                    .ToListAsync();

            //wait for the task to finish
            Task.WaitAny(decksRequest);

            //return the result
            return decksRequest.Result;
        }
    }
}