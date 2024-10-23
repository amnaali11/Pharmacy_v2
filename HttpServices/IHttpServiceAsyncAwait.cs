using Pharmacy_v2.DTOs;

namespace Pharmacy.HttpServices
{
    public interface IHttpServiceAsyncAwait
    {
        public  Task<string> GetToken();
        public Task<string> OrderRegistation(string token,int amount);
        public Task<string> PaymentKey(string token, string orderId,Data data );

    }
}
