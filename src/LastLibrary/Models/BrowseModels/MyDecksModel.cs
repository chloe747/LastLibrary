using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Models.BrowseModels
{
    public class MyDecksModel
    {
        public ICollection<DeckModel> UsersDecks { get; set; }
    }
}
