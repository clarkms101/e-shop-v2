using e_shop_api.Core.Enumeration;
using EShop.Entity.DataBase.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.Entity.DataBase
{
    public class EShopDbContext : DbContext
    {
        public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin
            modelBuilder.Entity<Admin>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Admin>().HasKey(s => s.Id);
            modelBuilder.Entity<Admin>().HasData(new Admin()
            {
                Id = 1,
                Account = "Clark",
                Password = "cc03e747a6afbbcbf8be7668acfebee5", // test123
                Permission = Permission.Public,
                CreationTime = DateTime.Now,
                LastModificationTime = DateTime.Now
            });

            // Coupon
            modelBuilder.Entity<Coupon>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Coupon>().HasKey(s => s.Id);

            // Order
            modelBuilder.Entity<Order>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().HasKey(s => s.Id);
            modelBuilder.Entity<Order>().HasIndex(s => s.SerialNumber);

            // OrderDetail
            modelBuilder.Entity<OrderDetail>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderDetail>().HasKey(s => s.Id);

            // Product
            modelBuilder.Entity<Product>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().HasKey(s => s.Id);

            // Category
            modelBuilder.Entity<Category>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>().HasKey(s => s.Id);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}