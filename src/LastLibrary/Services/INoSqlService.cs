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
        HttpStatusCode DeleteDeck(string deckId);
    }
}
