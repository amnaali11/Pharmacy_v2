using System.ComponentModel.DataAnnotations;

namespace Pharmacy_v2.DTOs
{
    public class AccountLoginDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember_me { get; set; } = false;

    }
}
