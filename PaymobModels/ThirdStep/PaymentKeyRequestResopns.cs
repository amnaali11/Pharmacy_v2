using Newtonsoft.Json;

namespace Pharmacy.PaymobModels.ThirdStep
{
    public class PaymentKeyRequestResopns
    {
        [JsonProperty("token")]
        public string? Token { get; set; }
    }
}
