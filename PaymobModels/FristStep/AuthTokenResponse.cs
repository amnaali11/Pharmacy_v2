using Newtonsoft.Json;

namespace Pharmacy.PaymobModels.FirstStep
{ 

    public class AuthTokenResponse
    {
        [JsonProperty("token")]
        public string ?Token { get; set; }
    }
}
