using Newtonsoft.Json;

namespace Pharmacy.PaymobModels.ThirdStep
{
    public class PaymentKeyRequest
    {
        [JsonProperty("auth_token")]
        public string ? AuthToken{ get; set; }
        [JsonProperty("amount_cents")]
        public decimal AmountContent { get; set; }
        [JsonProperty("expiration")]
        public int Expiration { get; set; }
        [JsonProperty("order_id")]
        public string ? OrderId { get; set; }
        [JsonProperty("billing_data")]
        public BillingData? BillingData { get; set; }
        [JsonProperty("currency")]
        public string? Currency { get; set; }
        [JsonProperty("integration_id")]
        public int IntegrationId { get; set; }
    }
}
