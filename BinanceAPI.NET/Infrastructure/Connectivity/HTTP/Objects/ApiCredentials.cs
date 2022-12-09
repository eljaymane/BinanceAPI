namespace BinanceAPI.NET.Infrastructure.Connectivity.HTTP.Objects
{
    public class ApiCredentials
    {
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public ApiCredentials(string apiKey, string apiSecret)
        {
            ApiKey = apiKey;
            ApiSecret = apiSecret;
        }
    }
}