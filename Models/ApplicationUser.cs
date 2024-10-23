using Microsoft.AspNetCore.Identity;
using Pharmacy_v2.Models;

namespace Pharmacy_v2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public int Age { get; set; }
        public virtual Bag Bag { get; set; }=new Bag();

    }
}
