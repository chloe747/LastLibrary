using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Services
{
    public interface INoSqlService
    {
        HttpStatusCode WriteDeck(DeckModel deckModel);
    }
}
