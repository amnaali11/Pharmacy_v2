using Newtonsoft.Json;
using Pharmacy.PaymobModels.FirstStep;

namespace Pharmacy.PaymobModels.SecondStep
{
    public class OrderRegistrationRequest
    {

        [JsonProperty("auth_token")]
        public string? AuthToken { get; set; }
        [JsonProperty("delivery_needed")]
        public bool DeliveryNeeded { get; set; }
        [JsonProperty("amount_cents")]
        public decimal AmountCents { get; set; }
        [JsonProperty("currency")]
        public string? Currency { get; set; }
        [JsonProperty("items")]
        public List<OrderItem> ?Items { get; set; }
    }
}
