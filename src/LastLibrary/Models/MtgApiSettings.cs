using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LastLibrary.Models
{
    public class MtgApiSettings
    {
        public Urls Urls { get; set; }
    }

    public class Urls
    {
        public string Cards { get; set; }
        public string Sets { get; set; }
        public string Types { get; set; }
    }
}
