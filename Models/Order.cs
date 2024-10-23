using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy_v2.Models
{
    public class Order   // data of medicine
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("BagId")]
        public int BagId {  get; set; }
        public string Name { get; set; }
        public decimal Cost {get; set; }
        public int Quantity { get; set; } = 1;
        public string? Image {  get; set; }
        public Bag bag { get; set; }
    }
}
