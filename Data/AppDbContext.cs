using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy_v2.Models;
using Pharmacy_v2.SignalR_Database;

namespace Pharmacy_v2.Data
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Medicine> Medicine { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Bag> bag { get; set; }
        public virtual DbSet<Conversation> conversation { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
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
