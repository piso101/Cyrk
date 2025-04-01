using Microsoft.EntityFrameworkCore;
using System;

namespace BackEndCyrk.Data
{

	public class ApplicationDbContext : DbContext
	{

        //public DbSet<OrderItem> OrderItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Konfikuracje Modeli
            // Konfiguracja dla OrderItem
            //modelBuilder.Entity<OrderItem>(oi => oi.Product).HasOne(oi => oi.Order).WithMany().HasForeignKey().OnDelete().IsUnique();

            //Nazewnictwo tabel
            //modelBuilder.Entity<OrderItem>().ToTable("Order_Items");
        }
    }
}
