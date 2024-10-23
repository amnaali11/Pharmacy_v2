using System.ComponentModel.DataAnnotations;

namespace Pharmacy_v2.DTOs
{
    public class Data
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public int Cost {  get; set; }
    }
}
