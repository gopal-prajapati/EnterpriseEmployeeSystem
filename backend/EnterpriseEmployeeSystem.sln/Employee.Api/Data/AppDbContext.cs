using EnterpriseEmployeeSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseEmployeeSystem.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<WebhookEvent> WebhookEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ItemCode)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasMaxLength(300)
                    .IsRequired();

                entity.Property(x => x.Amount)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Currency)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.HasOne(x => x.Employee)
                    .WithMany()
                    .HasForeignKey(x => x.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Gateway)
                    .HasMaxLength(50)
                    .IsRequired();

                entity.Property(x => x.GatewayOrderId)
                    .HasMaxLength(200);

                entity.Property(x => x.GatewayPaymentId)
                    .HasMaxLength(200);

                entity.Property(x => x.Amount)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Currency)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.Property(x => x.GatewayErrorCode)
                    .HasMaxLength(100);

                entity.Property(x => x.GatewayErrorDescription)
                    .HasMaxLength(500);

                entity.HasOne(x => x.Purchase)
                    .WithMany(x => x.Payments)
                    .HasForeignKey(x => x.PurchaseId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ItemCode)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(x => x.ItemCode)
                    .IsUnique();

                entity.Property(x => x.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.Price)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Currency)
                    .HasMaxLength(3)
                    .IsRequired();

                entity.HasData(
    new Product
    {
        Id = 1,
        ItemCode = "PREMIUM_REPORT",
        Name = "Premium Employee Report",
        Price = 499.00m,
        Currency = "INR",
        IsActive = true,
        CreatedAtUtc = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAtUtc = new DateTime(2026, 8, 28, 0, 0, 0, DateTimeKind.Utc)
    });
            });

            modelBuilder.Entity<WebhookEvent>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.EventId)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(x => x.EventType)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(x => x.EventId)
                    .IsUnique();
            });
        }


    }
}
