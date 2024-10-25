using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy_v2.Models
{
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string Name { get; set; }
        
        public string Message { get; set; }
        public DateTime time { get; set; } = DateTime.Now;
        public kind Kind { get; set; } = kind.AllGroups;
        public string? Gname { get; set; } = null;
        public string? IsSender { get; set; }
        public bool Deleted { get; set; } = false;
    }
}
