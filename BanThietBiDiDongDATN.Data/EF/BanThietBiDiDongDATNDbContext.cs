using BanThietBiDiDongDATN.Data.configurations;
using BanThietBiDiDongDATN.Data.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BanThietBiDiDongDATN.Data.EF
{
    public class BanThietBiDiDongDATNDbContext: IdentityDbContext<AppUser, AppRoles, Guid>
    {
        public BanThietBiDiDongDATNDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(builder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfigurations());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppRolesConfiguration());
            modelBuilder.ApplyConfiguration(new NewsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new RatingProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOptionConfiguration());
            modelBuilder.ApplyConfiguration(new WareHouseConfiguration());

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);



        }
        public DbSet<WareHouse> wareHouses { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Contact> contacts { get; set; }
        public DbSet<RatingProduct> ratingProducts { get; set; }
        public DbSet<Voucher> vouchers { get; set; }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> productImgs { get; set; }
        public DbSet<ProductOption> productOptions { get; set; }
        public DbSet<AppUser> appUsers { get; set; }
    }
}
