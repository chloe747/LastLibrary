using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Models.DeckManagerViewModel
{
    public class Deck
    {
        [Required]
        [RegularExpression("([a-zA-Z0-9 ]+)", ErrorMessage = "Alphanumeric Characters Only")]
        public string DeckName { get; set; }
    }
}
