namespace LastLibrary.Models.ConfigurationModels
{
    public class MtgApiConfiguration
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
