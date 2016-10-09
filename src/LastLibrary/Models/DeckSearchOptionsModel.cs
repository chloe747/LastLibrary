using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Models
{
    public class DeckSearchOptionsModel
    {
        public string UserName { get; set; }
        public string DeckName { get; set; }

        public bool IsBlue { get; set; }
        public bool IsRed { get; set; }
        public bool IsGreen { get; set; }
        public bool IsWhite { get; set; }
        public bool IsBlack { get; set; }
    }
}
