using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using restauracja_wpf.Models;

namespace restauracja_wpf.Data
{
    public class RestaurantContext : DbContext
    {
        private readonly IEncryptionProvider _provider;

        public RestaurantContext(DbContextOptions options) : base(options)
        {
            this._provider = new GenerateEncryptionProvider("dupa1234dupa1234dupa1234dupa1234");
        }

        public DbSet<Dish> Menu { get; set; }
        public DbSet<DishOrder> DishOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<RegularClient> RegularCustomers { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEncryption(_provider);

            modelBuilder.Entity<Dish>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<DishOrder>()
                .HasKey(dor => new { dor.Id, dor.DishId });

            modelBuilder.Entity<DishOrder>()
                .HasOne(dor => dor.Order)
                .WithMany(o => o.DishOrders)
                .HasForeignKey(dor => dor.Id);

            modelBuilder.Entity<DishOrder>()
                .HasOne(dor => dor.Dish)
                .WithMany(d => d.DishOrders)
                .HasForeignKey(dor => dor.DishId);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.StatusId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.RegularCustomer)
                .WithMany(rc => rc.Orders)
                .HasForeignKey(o => o.RegularCustomerId);

            modelBuilder.Entity<OrderStatus>()
                .HasKey(os => os.Id);

            modelBuilder.Entity<RegularClient>()
                .HasKey(rc => rc.Id);

            modelBuilder.Entity<RegularClient>()
                .HasOne(rc => rc.Discount)
                .WithMany(d => d.RegularCustomers)
                .HasForeignKey(rc => rc.DiscountId);

            modelBuilder.Entity<Discount>()
                .HasKey(d => d.Id);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Role>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Reservation)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.ReservationId);

            modelBuilder.Entity<Table>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Table>()
                .HasMany(t => t.Reservations)
                .WithMany(r => r.Tables)
                .UsingEntity(j => j.ToTable("Reservations_Tables"));

            modelBuilder.Entity<Table>()
                .HasMany(t => t.Orders)
                .WithMany(o => o.Tables)
                .UsingEntity(j => j.ToTable("Orders_Tables"));

            modelBuilder.Entity<Restaurant>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Restaurant)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RestaurantId);

            modelBuilder.Entity<Table>()
                .HasOne(t => t.Restaurant)
                .WithMany(r => r.Tables)
                .HasForeignKey(t => t.RestaurantId);
        }
    }
}
