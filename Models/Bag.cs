using Pharmacy_v2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy_v2.Models
{
    public class Bag
    {

        [Key]
        public int Id { get; set; }
       
        [ForeignKey ("UserId")]
        public string UserId { get; set; }
        public double Cost {  get; set; }
        public DateTime Buying_Date { get; set; }= DateTime.Now;
         
        public List<Order> Orders { get; set; }
        public ApplicationUser User { get; set; }

    }
}
