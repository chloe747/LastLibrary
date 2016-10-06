using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace LastLibrary.Models.DeckManagerViewModel
{
    [BsonIgnoreExtraElements]
    public class DeckModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Alphanumeric Characters Only")]
        public string DeckName { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public bool IsPublic { get; set; }
        public float? Rating { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ColourSpread> ColourSpread { get; set; }

        [Required]
        public ICollection<CardsInDeck> Cards { get; set; }
    }

    public class ColourSpread
    {
        public string Colour { get; set; }
        public double Percentage { get; set; }
    }

    public class CardsInDeck
    {
        public int Amount { get; set; }
        public CardModel Card { get; set; }
    }
}