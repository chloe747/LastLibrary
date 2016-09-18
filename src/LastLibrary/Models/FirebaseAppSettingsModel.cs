namespace LastLibrary.Models
{
    public class FirebaseAppSettingsModel
    {
        public string Secret { get; set; }
        public string ApiKey { get; set; }
        public string AuthDomain { get; set; }
        public string DatabaseUrl { get; set; }
        public string StorageBucket { get; set; }
        public string MessagingSenderId { get; set; }
    }
}
