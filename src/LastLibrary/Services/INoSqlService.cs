using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Services
{
    public interface INoSqlService
    {
        HttpStatusCode WriteDeck(DeckModel deckModel);
        ICollection<DeckModel> GetDecksForUser(string userName);
        ICollection<DeckModel> GetDecksByDeckName(string deckName);
        ICollection<DeckModel> GetDecksByUserNameAndDeckName(string userName, string deckName);
        DeckModel GetDeckById(string deckId);
        HttpStatusCode DeleteDeck(string deckId);
        HttpStatusCode UpdateDeckById(string deckId, DeckModel replacementDeck);
        HttpStatusCode AddCommentToDeck(CommentData comment, string deckId);
    }
}
