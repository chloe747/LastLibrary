using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LastLibrary.Models;
using LastLibrary.Models.DeckManagerViewModel;
using Microsoft.CodeAnalysis.Differencing;

namespace LastLibrary.Helpers
{
    public class DeckBuilderHelper
    {
        //function that calculated the colour spread of a deck
        public ICollection<ColourSpread> CalculateColourSpread(ICollection<CardsInDeck> cards)
        {
            double red = 0.0;
            double green = 0.0;
            double white = 0.0;
            double blue = 0.0;
            double black = 0.0;
            double totalCards = 0.0;

            //iterate over each card and add increment the colour cound with each colour found
            foreach (var cardData in cards)
            {
                //iterate over every entry in the Colours field in the cards
                foreach (var colour in cardData.Card.Colors)
                {
                    if (string.Compare(colour, "red", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        red += cardData.Amount;
                    }
                    else if (string.Compare(colour, "green", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        green += cardData.Amount;
                    }
                    else if (string.Compare(colour, "white", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        white += cardData.Amount;
                    }
                    else if (string.Compare(colour, "blue", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        blue += cardData.Amount;
                    }
                    else if (string.Compare(colour, "black", StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        black += cardData.Amount;
                    }
                }

                totalCards += cardData.Amount;
            }

            //calculate the percentage each colour takes in the deck
            var colours = new Collection<ColourSpread>();

            if (red > 0)
            {
                var percentage = (red / totalCards) * 100;
                var spread = new ColourSpread()
                {
                    Colour = "Red",
                    Percentage = percentage
                };
                colours.Add(spread);
            }
            if (green > 0)
            {
                var percentage = (green / totalCards) * 100;
                var spread = new ColourSpread()
                {
                    Colour = "Green",
                    Percentage = percentage
                };
                colours.Add(spread);
            }
            if (white > 0)
            {
                var percentage = (white / totalCards) * 100;
                var spread = new ColourSpread()
                {
                    Colour = "White",
                    Percentage = percentage
                };
                colours.Add(spread);
            }
            if (blue > 0)
            {
                var percentage = (blue / totalCards) * 100;
                var spread = new ColourSpread()
                {
                    Colour = "Blue",
                    Percentage = percentage
                };
                colours.Add(spread);
            }
            if (black > 0)
            {
                var percentage = (black / totalCards) * 100;
                var spread = new ColourSpread()
                {
                    Colour = "Black",
                    Percentage = percentage
                };
                colours.Add(spread);
            }
            return colours;
        }

    }
}
