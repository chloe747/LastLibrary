using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models.DeckManagerViewModel;

namespace LastLibrary.Models.BrowseModels
{
    public class DeckViewModel
    {
        public DeckModel Deck { get; set; }
        public string DeckId { get; set; }
        public bool IsCreator { get; set; }
    }
}
