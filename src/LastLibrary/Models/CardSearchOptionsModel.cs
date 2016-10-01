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
        public int PowerLessThan { get; set; }
        public int PowerLessThanEqualsTo { get; set; }
        public int PowerEquals { get; set; }
        public int PowerGreaterThan { get; set; }
        public int PowerGreaterThanEqualsTo { get; set; }

        //Toughness options
        public int ToughnessLessThan { get; set; }
        public int ToughnessLessThanEqualsTo { get; set; }
        public int ToughnessEquals { get; set; }
        public int ToughnessGreaterThan { get; set; }
        public int ToughnessGreaterThanEqualsTo { get; set; }

        //Converted Mana Cost options
        public int CmcLessThan { get; set; }
        public int CmcLessThanEqualsTo { get; set; }
        public int CmcEquals { get; set; }
        public int CmcGreaterThan { get; set; }
        public int CmcGreaterThanEqualsTo { get; set; }

        //Card Rarity options
        [RegularExpression("^Mythic Rare$|^Rare$|^Common$|^Uncommon$")]
        public string Rarity { get; set; }
    }
}
