using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Entities;

namespace Infrastructure.Data;

public class ECommerceDbContext : IdentityDbContext<Customer>
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    // Core Entities
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    
    // Address & Shipping
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<ShippingMethod> ShippingMethods => Set<ShippingMethod>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    
    // Payments
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    
    // Promotions
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();
    public DbSet<PromotionRule> PromotionRules => Set<PromotionRule>();
    
    // Wishlists
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    
    // Audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<UserActivity> UserActivities => Set<UserActivity>();
    
    // Cart & Inventory
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Inventory> Inventory => Set<Inventory>();
    public DbSet<InventoryReservation> InventoryReservations => Set<InventoryReservation>();
    
    // Reviews
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            entity.HasOne(p => p.Category)
                  .WithMany(c => c.Products)
                  .HasForeignKey(p => p.CategoryId);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.HasOne(c => c.Parent)
                  .WithMany(c => c.SubCategories)
                  .HasForeignKey(c => c.ParentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Customer entity (extends IdentityUser)
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.FirstName).HasMaxLength(100);
            entity.Property(c => c.LastName).HasMaxLength(100);
            
            // Configure relationships
            entity.HasMany(c => c.Addresses)
                  .WithOne(a => a.Customer)
                  .HasForeignKey(a => a.CustomerId);
            
            entity.HasMany(c => c.Orders)
                  .WithOne(o => o.Customer)
                  .HasForeignKey(o => o.CustomerId);
            
            entity.HasMany(c => c.Wishlists)
                  .WithOne(w => w.Customer)
                  .HasForeignKey(w => w.CustomerId);
        });

        // Configure Address entity
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Line1).IsRequired().HasMaxLength(256);
            entity.Property(a => a.City).IsRequired().HasMaxLength(100);
            entity.Property(a => a.State).IsRequired().HasMaxLength(100);
            entity.Property(a => a.PostalCode).IsRequired().HasMaxLength(20);
            entity.Property(a => a.Country).IsRequired().HasMaxLength(100);
        });

        // Configure ShippingMethod entity
        modelBuilder.Entity<ShippingMethod>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
            entity.Property(s => s.CarrierCode).IsRequired().HasMaxLength(50);
            entity.Property(s => s.FlatRate).HasColumnType("decimal(18,2)");
        });

        // Configure Shipment entity
        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.TrackingNumber).HasMaxLength(100);
            entity.HasOne(s => s.Order)
                  .WithMany()
                  .HasForeignKey(s => s.OrderId);
            entity.HasOne(s => s.ShippingMethod)
                  .WithMany()
                  .HasForeignKey(s => s.ShippingMethodId);
        });

        // Configure PaymentMethod entity
        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.HasKey(pm => pm.Id);
            entity.Property(pm => pm.Provider).IsRequired().HasMaxLength(50);
            entity.Property(pm => pm.MaskedDetails).HasMaxLength(100);
        });

        // Configure Payment entity
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Amount).HasPrecision(18, 2);
            entity.Property(p => p.Currency).HasMaxLength(3);
            entity.HasOne(p => p.Order)
                  .WithMany(o => o.Payments)
                  .HasForeignKey(p => p.OrderId);
        });

        // Configure PaymentTransaction entity
        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(pt => pt.Id);
            entity.Property(pt => pt.GatewayTransactionId).HasMaxLength(100);
            entity.HasOne(pt => pt.Payment)
                  .WithMany(p => p.Transactions)
                  .HasForeignKey(pt => pt.PaymentId);
        });

        // Configure Coupon entity
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Code).IsUnique();
            entity.Property(c => c.Code).IsRequired().HasMaxLength(50);
            entity.Property(c => c.DiscountValue).HasPrecision(18, 2);
            entity.Property(c => c.MinOrderAmount).HasPrecision(18, 2);
        });

        // Configure CouponUsage entity
        modelBuilder.Entity<CouponUsage>(entity =>
        {
            entity.HasKey(cu => cu.Id);
            entity.HasOne(cu => cu.Coupon)
                  .WithMany(c => c.Usages)
                  .HasForeignKey(cu => cu.CouponId);
            entity.HasOne(cu => cu.Order)
                  .WithMany()
                  .HasForeignKey(cu => cu.OrderId);
        });

        // Configure PromotionRule entity
        modelBuilder.Entity<PromotionRule>(entity =>
        {
            entity.HasKey(pr => pr.Id);
            entity.Property(pr => pr.RuleExpression).HasColumnType("nvarchar(max)");
        });

        // Configure Wishlist entity
        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
        });

        // Configure WishlistItem entity
        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(wi => wi.Id);
            entity.HasOne(wi => wi.Product)
                  .WithMany()
                  .HasForeignKey(wi => wi.ProductId);
        });

        // Configure AuditLog entity
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.EntityName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.EntityId).IsRequired().HasMaxLength(100);
            entity.Property(a => a.OldValues).HasColumnType("nvarchar(max)");
            entity.Property(a => a.NewValues).HasColumnType("nvarchar(max)");
        });

        // Configure UserActivity entity
        modelBuilder.Entity<UserActivity>(entity =>
        {
            entity.HasKey(ua => ua.Id);
            entity.Property(ua => ua.IpAddress).HasMaxLength(50);
            entity.Property(ua => ua.UserAgent).HasMaxLength(500);
        });

        // Configure Order entity
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(o => o.SubTotal).HasPrecision(18, 2);
            entity.Property(o => o.TaxAmount).HasPrecision(18, 2);
            entity.Property(o => o.ShippingAmount).HasPrecision(18, 2);
            entity.Property(o => o.DiscountAmount).HasPrecision(18, 2);
            entity.Property(o => o.GrandTotal).HasPrecision(18, 2);
            entity.HasOne(o => o.BillingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.BillingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(o => o.ShippingAddress)
                  .WithMany()
                  .HasForeignKey(o => o.ShippingAddressId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure OrderItem entity
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(oi => oi.Id);
            entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            entity.HasOne(oi => oi.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(oi => oi.OrderId);
            entity.HasOne(oi => oi.Product)
                  .WithMany()
                  .HasForeignKey(oi => oi.ProductId);
        });

        // Configure InventoryReservation entity
        modelBuilder.Entity<InventoryReservation>(entity =>
        {
            entity.HasKey(ir => ir.Id);
            entity.HasOne(ir => ir.Product)
                  .WithMany()
                  .HasForeignKey(ir => ir.ProductId);
            entity.HasOne(ir => ir.Order)
                  .WithMany()
                  .HasForeignKey(ir => ir.OrderId);
        });

        // Configure Cart entity
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(c => c.Id);
        });

        // Configure CartItem entity
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(ci => ci.Id);
            entity.HasOne(ci => ci.Product)
                  .WithMany()
                  .HasForeignKey(ci => ci.ProductId);
        });

        // Configure Inventory entity
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(i => i.ProductId);
            entity.Property(i => i.Quantity).IsRequired();
            entity.Property(i => i.ReservedQuantity).HasDefaultValue(0);
            entity.Property(i => i.LowStockThreshold).HasDefaultValue(10);
        });

        // Configure Review entity
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Rating).IsRequired();
            entity.Property(r => r.Comment).HasMaxLength(1000);
        });

        // Global query filter for soft delete
        modelBuilder.Entity<BaseEntity>()
            .HasQueryFilter(e => e.IsActive);
    }
}
