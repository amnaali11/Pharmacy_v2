using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Models;
using System.Reflection.Emit;

namespace Pharmacy_v2.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Bag> bag { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>()
               .HasMany(c => c.Medicines)
               .WithOne(m => m.category) 
               .HasForeignKey(m => m.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Bag>().HasOne(x=>x.User).WithOne(x=>x.Bag).OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);


        }
    }
}
