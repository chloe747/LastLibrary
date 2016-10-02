﻿namespace LastLibrary.Models.ConfigurationModels
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
