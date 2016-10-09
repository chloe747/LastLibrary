using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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

            return result.IsFaulted || result.IsCanceled
                ? HttpStatusCode.InternalServerError
                : HttpStatusCode.OK;
        }


        public ICollection<DeckModel> GetDecksForUser(string userName)
        {
            //construct a filter
            var filter = Builders<DeckModel>.Filter.Regex(u => u.Creator,
                new BsonRegularExpression("/^" + userName + "$/i"));

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
            var filter = Builders<DeckModel>.Filter.Regex(u => u.DeckName,
                new BsonRegularExpression("/^" + deckName + "$/i"));

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
            FilterDefinition<DeckModel> filter;
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
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            return result.Result;
        }

        public HttpStatusCode DeleteDeck(string deckId)
        {
            var filter = Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            //delete the deck using the mongoDB driver
            var result = DecksCollection.DeleteManyAsync(filter);

            Task.WaitAny(result);

            return result.IsFaulted || result.IsCanceled
                ? HttpStatusCode.InternalServerError
                : HttpStatusCode.OK;
        }

        public HttpStatusCode UpdateDeckById(string deckId, DeckModel replacementDeck)
        {
            //create the BSON id model to find
            FilterDefinition<DeckModel> filter;
            try
            {
                filter = Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            //find the matching deck ID
            var result = DecksCollection.ReplaceOneAsync(filter, replacementDeck);

            Task.WaitAny(result);

            //error handling
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            return HttpStatusCode.OK;
        }

        public HttpStatusCode AddCommentToDeck(CommentData comment, string deckId)
        {
            FilterDefinition<DeckModel> filter;
            try
            {
                filter = Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var update = Builders<DeckModel>.Update.Push(e => e.Comments, comment);
            var result = DecksCollection.UpdateOneAsync(filter, update);

            Task.WaitAny(result);

            //error handling
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            return HttpStatusCode.OK;
        }

        public HttpStatusCode RateDeckGetDeckById(string deckId, RatingData rating)
        {
            //get the id model for the deck
            FilterDefinition<DeckModel> idFilter;
            try
            {
                idFilter = Builders<DeckModel>.Filter.Eq("_id", ObjectId.Parse(deckId));
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var ratingFilter = Builders<DeckModel>.Filter.ElemMatch(deck => deck.Ratings,
                deckRating => deckRating.UserName == rating.UserName);
            var idAndRatingFilter = Builders<DeckModel>.Filter.And(idFilter, ratingFilter);

            //create the updator
            var ratingSetter = Builders<DeckModel>.Update.Set("Ratings.$", rating);
            //update the user's rating, or create it if they don't exist
            var result = DecksCollection.UpdateOneAsync(idAndRatingFilter, ratingSetter);

            Task.WaitAny(result);

            //error handling
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            //check to see if anything was modified
            if (result.Result.ModifiedCount == 0)
            {
                //if nothing was modified, push a new element to the ratings array
                var ratingsPusher = Builders<DeckModel>.Update.Push(e => e.Ratings, rating);
                result = DecksCollection.UpdateOneAsync(idFilter, ratingsPusher);

                Task.WaitAny(result);

                //error handling
                if (result.IsFaulted || result.IsCanceled)
                    throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return HttpStatusCode.OK;
        }

        public ICollection<DeckModel> GetDecksByTopRated(int amount)
        {
            //get all decks by is public
            var result = DecksCollection.Find(deck => deck.IsPublic && (deck.Ratings != null)).ToListAsync();

            Task.WaitAny(result);

            //error handling
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            //now, get the average ratings of the decks
            var averageSortedDecks =
                result.Result.OrderByDescending(e => e.Ratings.Select(r => r.Rating == null ? 0 : r.Rating).Average())
                    .Take(amount)
                    .ToList();

            return averageSortedDecks;
        }

        public ICollection<DeckModel> GetDecksByNewest(int amount)
        {
            //get all decks by is public
            var result = DecksCollection.Find(deck => deck.IsPublic).ToListAsync();

            Task.WaitAny(result);

            //error handling
            if (result.IsFaulted || result.IsCanceled)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);

            //now, get the newest decks
            var newestDecks =
                result.Result.OrderByDescending(e => e.CreationDate)
                    .Take(amount)
                    .ToList();

            return newestDecks;
        }
    }
}