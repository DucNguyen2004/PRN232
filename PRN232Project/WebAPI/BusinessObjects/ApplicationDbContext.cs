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

        // DbSets for all your entities
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponUsage> CouponUsages { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
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
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between User and Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("user_roles"));

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description);
            });

            // UserAddress Configuration
            modelBuilder.Entity<UserAddress>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Address).HasMaxLength(256).IsRequired();
                entity.Property(e => e.IsDefault).IsRequired();

                entity.HasOne(ua => ua.User)
                      .WithMany(u => u.Addresses)
                      .HasForeignKey(ua => ua.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Category Configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(256).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.CreateAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Sold).HasDefaultValue(0);
                entity.Property(e => e.Status);
                entity.Property(e => e.PrevStatus);

                entity.HasOne(p => p.Category)
                      .WithMany()
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ProductImage Configuration
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Image).HasMaxLength(256).IsRequired();

                entity.HasOne(pi => pi.Product)
                      .WithMany(p => p.ProductImages)
                      .HasForeignKey(pi => pi.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Option Configuration
            modelBuilder.Entity<Option>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);
            });

            // OptionValue Configuration
            modelBuilder.Entity<OptionValue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name);

                entity.HasOne(ov => ov.Option)
                      .WithMany(o => o.OptionValues)
                      .HasForeignKey(ov => ov.OptionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProductOption Configuration
            modelBuilder.Entity<ProductOption>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(po => po.Product)
                      .WithMany(p => p.ProductOptions)
                      .HasForeignKey(po => po.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(po => po.OptionValue)
                      .WithMany(o => o.ProductOptions)
                      .HasForeignKey(po => po.OptionValueId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CartItem Configuration
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

                // Ensure unique cart item per user-product combination
                //entity.HasIndex(ci => new { ci.UserId, ci.ProductId }).IsUnique();
            });
            modelBuilder.Entity<CartItem>()
                .HasMany(ci => ci.ProductOptions)
                .WithMany(po => po.CartItems)
                .UsingEntity<Dictionary<string, object>>(
                    "CartItemProductOptions",
                    j => j
                        .HasOne<ProductOption>()
                        .WithMany()
                        .HasForeignKey("ProductOptionsId")
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne<CartItem>()
                        .WithMany()
                        .HasForeignKey("CartItemsId")
                        .OnDelete(DeleteBehavior.Restrict)
                );

            // WishlistItem Configuration
            modelBuilder.Entity<WishlistItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(wi => wi.User)
                      .WithMany()
                      .HasForeignKey(wi => wi.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(wi => wi.Product)
                      .WithMany(p => p.WishlistItems)
                      .HasForeignKey(wi => wi.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure unique wishlist item per user-product combination
                entity.HasIndex(wi => new { wi.UserId, wi.ProductId }).IsUnique();
            });

            // Coupon Configuration
            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Code);
                entity.Property(c => c.Name);
                entity.Property(c => c.Description);
                entity.Property(c => c.ValueType);
                entity.Property(c => c.Value);
                entity.Property(c => c.MinOrderValue);
                entity.Property(c => c.Status);

                entity.HasOne(c => c.Product)
                    .WithMany()
                    .HasForeignKey(c => c.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Category)
                    .WithMany()
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // CouponUsage Configuration
            modelBuilder.Entity<CouponUsage>(entity =>
            {
                entity.HasKey(cu => cu.Id);
                entity.HasIndex(cu => new { cu.CouponId, cu.UserId }).IsUnique();

                entity.HasOne(cu => cu.Coupon)
                    .WithMany(c => c.Usages)
                    .HasForeignKey(cu => cu.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cu => cu.User)
                    .WithMany(u => u.CouponUsages)
                    .HasForeignKey(cu => cu.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cu => cu.UsedAt)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.ShippingAddress).IsRequired();
                entity.Property(e => e.OrderStatus).IsRequired();
                entity.Property(e => e.Message);
                entity.Property(e => e.DiscountPrice).HasDefaultValue(0);
                entity.Property(e => e.CancelReason);

                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Coupon)
                      .WithMany()
                      .HasForeignKey(o => o.CouponId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // OrderDetail Configuration
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
                j => j
                    .HasOne<ProductOption>()
                    .WithMany()
                    .HasForeignKey("ProductOptionsId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<OrderDetail>()
                    .WithMany()
                    .HasForeignKey("OrderDetailsId")
                    .OnDelete(DeleteBehavior.Cascade),
                         je =>
                         {
                             je.HasKey("OrderDetailsId", "ProductOptionsId");
                             //je.Property<int>("OrderDetailsId");
                             //je.Property<int>("ProductOptionsId");
                         }
            );

            // RefreshToken Configuration
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.Expiry).IsRequired();

                entity.HasOne(rt => rt.User)
                      .WithMany()
                      .HasForeignKey(rt => rt.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ensure unique token
                entity.HasIndex(rt => rt.Token).IsUnique();
            });

            // Feedback Configuration
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.SentAt).IsRequired();

                // Note: IsRead is private in your model, you may need to make it public
                // or configure it differently if you want to map it to the database
            });

            //--------------------------------------------------------------------------------//

            //modelBuilder.Entity<Role>().HasData(
            //    new Role { Id = 1, Name = "admin", Description = "Administrator role with full access" },
            //    new Role { Id = 2, Name = "user", Description = "Regular user role with limited access" }
            //);

            //modelBuilder.Entity<User>().HasData(
            //    new User { Id = 1, Username = "test1", Password = "12345@", IsHavePassword = true, Fullname = "Test 1", Email = "test1@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 2, Username = "test2", Password = "12345@", IsHavePassword = true, Fullname = "Test 2", Email = "test2@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 3, Username = "test3", Password = "12345@", IsHavePassword = true, Fullname = "Test 3", Email = "test3@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 4, Username = "test4", Password = "12345@", IsHavePassword = true, Fullname = "Test 4", Email = "test4@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 5, Username = "test5", Password = "12345@", IsHavePassword = true, Fullname = "Test 5", Email = "test5@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 6, Username = "test6", Password = "12345@", IsHavePassword = true, Fullname = "Test 6", Email = "test6@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 7, Username = "test7", Password = "12345@", IsHavePassword = true, Fullname = "Test 7", Email = "test7@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 8, Username = "test8", Password = "12345@", IsHavePassword = true, Fullname = "Test 8", Email = "test8@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 9, Username = "test9", Password = "12345@", IsHavePassword = true, Fullname = "Test 9", Email = "test9@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) },
            //    new User { Id = 10, Username = "test10", Password = "12345@", IsHavePassword = true, Fullname = "Test 10", Email = "test10@gm.co", Phone = "0123456789", Dob = new DateTime(2001, 6, 3), Gender = "Male", Status = "ACTIVE", IsActivated = true, CreatedAt = new DateTime(2025, 6, 25), UpdatedAt = new DateTime(2025, 6, 25) }
            //);
        }
    }
}
