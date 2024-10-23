using Newtonsoft.Json;

namespace Pharmacy.PaymobModels.FirstStep{
    public class ApiKey
    {
        [JsonProperty("api-key")]
        public string Key { set; get; }
    }
}