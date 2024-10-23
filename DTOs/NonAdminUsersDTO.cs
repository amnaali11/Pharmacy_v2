using System.ComponentModel.DataAnnotations;

namespace Pharmacy_v2.DTOs
{
    public class NonAdminUsersDTO
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}