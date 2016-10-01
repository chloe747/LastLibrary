using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Models
{
    public class CardSearchOptionsModel
    {
         //Colour options
        public bool IsBlue { get; set; }
        public bool IsRed { get; set; }
        public bool IsGreen { get; set; }
        public bool IsWhite { get; set; }
        public bool IsBlack { get; set; }
        public bool IsColorless { get; set; }

        //Power options
        public string Power { get; set; }
        [RegularExpression("^gte$|^lte$|^lt$|^gt$")]
        public string PowerOperator { get; set; }

        //Toughness options
        public string Toughness { get; set; }
        [RegularExpression("^gte$|^lte$|^lt$|^gt$")]
        public string ToughnessOperator { get; set; }

        //Converted Mana Cost options
        public string Cmc { get; set; }
        [RegularExpression("^gte$|^lte$|^lt$|^gt$")]
        public string CmcOperator { get; set; }

        //Card Rarity options
        [RegularExpression("^Mythic Rare$|^Rare$|^Common$|^Uncommon$")]
        public string Rarity { get; set; }
    }
}
