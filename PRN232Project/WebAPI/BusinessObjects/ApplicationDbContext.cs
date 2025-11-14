using Microsoft.EntityFrameworkCore;

namespace BusinessObjects
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<OptionValue> OptionValues { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItem { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER–ROLE Many-to-Many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("user_roles"));

            // Role Config
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            });

            // Category Config
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            // Product Config
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(256).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.CreateAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Sold).HasDefaultValue(0);

                entity.HasOne(p => p.Category)
                      .WithMany()
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ProductImage
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Image).HasMaxLength(256).IsRequired();
                entity.HasOne(pi => pi.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(pi => pi.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Option
            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            // OptionValue
            modelBuilder.Entity<OptionValue>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(ov => ov.Option)
                      .WithMany(o => o.OptionValues)
                      .HasForeignKey(ov => ov.OptionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProductOption
            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(po => po.Product)
                      .WithMany(p => p.ProductOptions)
                      .HasForeignKey(po => po.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(po => po.OptionValue)
                      .WithMany(ov => ov.ProductOptions)
                      .HasForeignKey(po => po.OptionValueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();

                entity.HasOne(ci => ci.User)
                      .WithMany()
                      .HasForeignKey(ci => ci.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ci => ci.Product)
                      .WithMany(p => p.CartItems)
                      .HasForeignKey(ci => ci.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CartItem>()
                .HasMany(ci => ci.ProductOptions)
                .WithMany(po => po.CartItems)
                .UsingEntity<Dictionary<string, object>>(
                    "CartItemProductOptions",
                    j => j.HasOne<ProductOption>().WithMany().HasForeignKey("ProductOptionsId"),
                    j => j.HasOne<CartItem>().WithMany().HasForeignKey("CartItemsId")
                );

            // Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.ShippingAddress).IsRequired();
                entity.Property(e => e.OrderStatus).IsRequired();
                entity.Property(e => e.DiscountPrice).HasDefaultValue(0);

                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Price).IsRequired();

                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(od => od.Product)
                      .WithMany()
                      .HasForeignKey(od => od.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<OrderDetail>()
                .HasMany(od => od.ProductOptions)
                .WithMany(po => po.OrderDetails)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderDetailProductOption",
                    j => j.HasOne<ProductOption>().WithMany().HasForeignKey("ProductOptionsId"),
                    j => j.HasOne<OrderDetail>().WithMany().HasForeignKey("OrderDetailsId")
                );

            // RefreshToken
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.Expiry).IsRequired();
                entity.HasIndex(e => e.Token).IsUnique();

                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
