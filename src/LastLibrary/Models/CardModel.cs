using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LastLibrary.Models
{
    public class CardsModel
    {
        public Collection<CardModel> Cards { get; set; }
    }

    public class CardModel
    {
        public string Name { get; set; }
        public Collection<string> Names { get; set; }
        public string ManaCost { get; set; }
        public int Cmc { get; set; }
        public Collection<string> Colors { get; set; }
        public string Type { get; set; }
        public Collection<string> SuperTypes { get; set; }
        public Collection<string> Types { get; set; }
        public Collection<string> SubTypes { get; set; }
        public string Rarity { get; set; }
        public string Set { get; set; }
        public string Text { get; set; }
        public string Artist { get; set; }
        public string Number { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public int MultiverseId { get; set; }
        public string ImageUrl { get; set; }
        public Collection<Rulings> Rulings { get; set; }
        public Collection<string> Printings { get; set; }
        public Collection<Legalities> Legalities { get; set; }
        public string OriginalText { get; set; }
        public string OriginalType { get; set; }
        public string Source { get; set; }
        public string Id { get; set; }

    }

    public class Rulings
    {
        public string Date { get; set; }
        public string Text { get; set; }
    }

    public class Legalities
    {
        public string Format { get; set; }
        public string Legality { get; set; }
    }
}
