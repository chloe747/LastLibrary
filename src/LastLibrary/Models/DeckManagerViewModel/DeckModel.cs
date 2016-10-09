using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LastLibrary.Models.DeckManagerViewModel
{
    [BsonIgnoreExtraElements]
    public class DeckModel
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]*$", ErrorMessage = "Alphanumeric Characters Only")]
        public string DeckName { get; set; }
        public string Description { get; set; }
        public string Creator { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<ColourSpread> ColourSpread { get; set; }

        [Required]
        public ICollection<CardsInDeck> Cards { get; set; }

        [BsonIgnoreIfDefault]
        public Collection<CommentData> Comments { get; set; }

        [BsonIgnoreIfDefault]
        public Collection<RatingData> Ratings { get; set; }
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

    public class CommentData
    {
        public string Comment { get; set; }
        public DateTime CommentDate { get; set; }
        public string Commenter { get; set; }
    }

    public class RatingData
    {
        [Required]
        [Range(0, 5)]
        public int? Rating { get; set; }
        public string UserName { get; set; }
    }
}