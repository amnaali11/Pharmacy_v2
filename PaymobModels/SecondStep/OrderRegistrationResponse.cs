using Newtonsoft.Json;

namespace Pharmacy.PaymobModels.SecondStep
{
    public class OrderRegistrationResponse
    {
        [JsonProperty("id")]
        public string? Id { get; set; }  
    }
}
