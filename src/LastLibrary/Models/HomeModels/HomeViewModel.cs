using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Models.HomeModels
{
    public class HomeViewModel
    {
        public ICollection<DeckModel> TopRatedDecks { get; set; }
        public ICollection<DeckModel> NewestDecks { get; set; }
    }
}
