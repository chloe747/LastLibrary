namespace LastLibrary.Models.ConfigurationModels
{
    public class MtgApiconfiguration
    {
        public MtgApiconfigurationUrls Urls { get; set; }
    }

    public class MtgApiconfigurationUrls
    {
        public string Cards { get; set; }
        public string Sets { get; set; }
        public string Types { get; set; }
    }
}
