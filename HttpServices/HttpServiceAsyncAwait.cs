using Newtonsoft.Json;
using Pharmacy.PaymobModels.FirstStep;
using Pharmacy.PaymobModels.SecondStep;
using Pharmacy.PaymobModels.ThirdStep;
using Pharmacy_v2.DTOs;
using System.Text;

namespace Pharmacy.HttpServices
{
    public class HttpServiceAsyncAwait :IHttpServiceAsyncAwait 
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public HttpServiceAsyncAwait(IConfiguration configuration, IHttpClientFactory http)
        {
            _configuration = configuration;
            _client = http.CreateClient();
        }
        public async Task<string> GetToken()
        {
            try
            {
                string URL = "https://accept.paymob.com/api/auth/tokens";
                string apikey = _configuration.GetValue<string>("ApiKey");
                var keyobject = new { api_key = apikey };
                var seralizedobj = new StringContent(JsonConvert.SerializeObject(keyobject), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(URL, seralizedobj);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<AuthTokenResponse>(data);
                    if (obj is not null)
                        if(obj.Token is not null)
                    return obj.Token;
                    return "error message";
                }
                return "error";
             
            }
            catch (Exception ex)
            {
                // if (ex.InnerException != null)
                // throw ex.InnerException;
                // throw  ex.Message.ToString();
                return ex.Message;


            }
          
        }

        public async Task<string> OrderRegistation(string token,int amount)
        {
            try { 
            string URL = "https://accept.paymob.com/api/ecommerce/orders";
            List<OrderItem> items = new();
               
                var RequestedData = new OrderRegistrationRequest
            {
                AmountCents = amount*100,
                AuthToken = token,
                DeliveryNeeded = false,
                Currency = "EGP",
                Items = items
            };
            var SerializedRequestedObj = new StringContent(JsonConvert.SerializeObject(RequestedData), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(URL, SerializedRequestedObj);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<OrderRegistrationResponse>(data);
                if (obj is not null)
                   if(obj.Id is not null)
                    return obj.Id;
                return "error";
            }
            return "error";
        }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> PaymentKey(string token, string orderId,Data data)
        {
            try {
                var URL = "https://accept.paymob.com/api/acceptance/payment_keys";
                var RequestedData = new PaymentKeyRequest
                {
                    AuthToken = token,
                    AmountContent = data.Cost*100,
                    Expiration = 4000,
                    OrderId = orderId,
                    BillingData = new BillingData
                    {
                        //RequiredData
                        FirstName =data.FirstName,
                        LastName = data.LastName,
                        PhoneNumber = data.Phone,
                        Email = data.Email,
                       
                        
                        Apartment = "NA",
                        Floor = "NA",
                        Street = "NA",
                        Building = "NA",
                        ShippingMethod = "NA",
                        PostalCode = "NA",
                        City = "NA",
                        Country = "NA",
                        State = "NA"
                    },
                    Currency = "EGP",
                    IntegrationId = _configuration.GetValue<int>("IntegrationId")
                };
                var SerializedRequestedObj = new StringContent(JsonConvert.SerializeObject(RequestedData), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(URL, SerializedRequestedObj);
                if (response.IsSuccessStatusCode) {
                    var data1 = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<PaymentKeyRequestResopns>(data1);
                    if (obj != null)
                    {
                        if (obj.Token is not null)
                            return obj.Token;
                    }
                    else
                    {
                        return "error";
                    }
                }
                return "error";


            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            }
        
    }
}
