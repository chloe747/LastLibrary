using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LastLibrary.Models.ConfigurationModels;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace LastLibrary.Services.MongoDb
{
    public class MongoDbService : INoSqlService
    {


        public MongoDbService(IOptions<MongoDbConfigurationModel> config)
        {
            //construct the connection URL
            ConnectionUrl = "mongodb://" + config.Value.UserName + ":" + config.Value.Password + config.Value.Url;
        }

        private string ConnectionUrl { get; }
        private IMongoCollection<DeckModel> DecksCollection
        {
            get
            {
                //connect to mongoDb
                var client = new MongoClient(ConnectionUrl);

                //connect to the database
                var database = client.GetDatabase("lastlibrary");

                return database.GetCollection<DeckModel>("Decks");
            }
        }

        public HttpStatusCode WriteDeck(DeckModel deckModel)
        {
            var result = DecksCollection.InsertOneAsync(deckModel);

            Task.WaitAny(result);

            return (result.IsFaulted) || (result.IsFaulted)
                ? HttpStatusCode.InternalServerError
                : HttpStatusCode.OK;
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

        public DeckModel GetDeckById(string deckId)
        {
            //create the BSON id model to find
            FilterDefinition<DeckModel>filter;
            try
            {
                filter = Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
            //find the matching deck ID
            var result = DecksCollection.Find(filter).SingleAsync();

            Task.WaitAny(result);

            //error handling
            if ((result.IsFaulted) || (result.IsFaulted))
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return result.Result;
        }

        public HttpStatusCode DeleteDeck(string deckId)
        {
            var filter= Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            //delete the deck using the mongoDB driver
            var result = DecksCollection.DeleteManyAsync(filter);

            Task.WaitAny(result);

            return (result.IsFaulted) || (result.IsFaulted)
                ? HttpStatusCode.InternalServerError
                : HttpStatusCode.OK;
        }

    }
}