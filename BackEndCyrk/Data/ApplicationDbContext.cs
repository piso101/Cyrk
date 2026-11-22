using Microsoft.EntityFrameworkCore;
using BackEndCyrk.Models;

namespace BackEndCyrk.Data
{
    public class ApplicationDbContext : DbContext
    {
        // DbSet dla każdego modelu
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja dla User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.user_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja dla Category
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.name)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.category_id)
                .OnDelete(DeleteBehavior.SetNull);

            // Konfiguracja dla Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.category_id)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Inventory)
                .WithOne(i => i.Product)
                .HasForeignKey<Inventory>(i => i.product_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja dla Order
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.order_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.order)
                .HasForeignKey<Payment>(p => p.order_id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipping)
                .WithOne(s => s.Order)
                .HasForeignKey<Shipping>(s => s.order_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Konfiguracja dla OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.product_id)
                .OnDelete(DeleteBehavior.Cascade);

            // Konwencje nazewnictwa tabel
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Order>().ToTable("orders");
            modelBuilder.Entity<OrderItem>().ToTable("order_items");
            modelBuilder.Entity<Payment>().ToTable("payments");
            modelBuilder.Entity<Inventory>().ToTable("inventory");
            modelBuilder.Entity<Shipping>().ToTable("shipping");
            modelBuilder.Entity<ShippingRate>().ToTable("shipping_rates");
        }
    }
}