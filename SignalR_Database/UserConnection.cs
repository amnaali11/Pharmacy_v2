using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy_v2.SignalR_Database
{
    public class UserConnection
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }

    }
}
