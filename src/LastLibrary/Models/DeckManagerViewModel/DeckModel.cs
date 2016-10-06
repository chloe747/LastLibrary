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
        [BsonElement]
        public string DeckName { get; set; }

        [BsonElement]
        public string Description { get; set; }
        [BsonElement]
        public string Creator { get; set; }
        [BsonElement]
        public bool IsPublic { get; set; }
        [BsonElement]
        public float? Rating { get; set; }
        [BsonElement]
        public DateTime CreationDate { get; set; }
        [BsonElement]
        public Collection<ColourSpread> ColourSpread { get; set; }

        [Required]
        [BsonElement]
        public Collection<CardsInDeck> Cards { get; set; }
    }

    public class ColourSpread
    {
        [BsonElement]
        public string Colour { get; set; }
        [BsonElement]
        public float Percentage { get; set; }
    }

    public class CardsInDeck
    {
        [BsonElement]
        public int Amount { get; set; }
        [BsonElement]
        public CardModel Card { get; set; }
    }
}