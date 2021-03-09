namespace Bibliotheque.Api.Helpers
{
    public class ApplicationSettings
    {
        public string JwtSecretKey { get; set }
        public string TokenExpireMinute { get; set; }

        public string ClientUrl { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }

        public string DefaultIV { get; set; }
        public string DefaultKey { get; set; }
    }
}
