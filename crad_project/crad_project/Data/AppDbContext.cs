using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crad_project.Models;

namespace crad_project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Mobile> Mobiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقة بين User و Address (عبر UserAddress)
            modelBuilder.Entity<UserAddress>()
                .HasKey(ua => new { ua.UserId, ua.AddressId });

            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAddress>()
                .HasOne(ua => ua.Address)
                .WithMany(a => a.UserAddresses)
                .HasForeignKey(ua => ua.AddressId);

            // العلاقة بين Address و Province
            modelBuilder.Entity<Address>()
                .HasOne(a => a.Province)
                .WithMany(p => p.Addresses)
                .HasForeignKey(a => a.ProvinceId);

            // العلاقة بين Order و User
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // العلاقة بين Product و Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            // العلاقة بين Payment و Order
            modelBuilder.Entity<Payments>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payments>(p => p.OrderId);

            // العلاقة بين Review و User
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // العلاقة بين Review و Product
            modelBuilder.Entity<Reviews>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId);

            // العلاقة بين OrderItems و Order
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // العلاقة بين OrderItems و Product
            modelBuilder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // العلاقة بين SubCategory و Category
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.Category)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Mobile>()
                .HasKey(m => m.MobileId);
            modelBuilder.Entity<Mobile>()
                .HasOne(m => m.SubCategory)
                .WithMany(sc => sc.Mobiles)
                .HasForeignKey(m => m.SubCategoryId);


        }
    }
}
