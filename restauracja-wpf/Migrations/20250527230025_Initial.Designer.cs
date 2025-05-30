﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using restauracja_wpf.Data;

#nullable disable

namespace restauracja_wpf.Migrations
{
    [DbContext(typeof(RestaurantContext))]
    [Migration("20250527230025_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderTable", b =>
                {
                    b.Property<int>("OrdersOrderId")
                        .HasColumnType("int");

                    b.Property<int>("TablesTableId")
                        .HasColumnType("int");

                    b.HasKey("OrdersOrderId", "TablesTableId");

                    b.HasIndex("TablesTableId");

                    b.ToTable("Orders_Tables", (string)null);
                });

            modelBuilder.Entity("ReservationTable", b =>
                {
                    b.Property<int>("ReservationsReservationId")
                        .HasColumnType("int");

                    b.Property<int>("TablesTableId")
                        .HasColumnType("int");

                    b.HasKey("ReservationsReservationId", "TablesTableId");

                    b.HasIndex("TablesTableId");

                    b.ToTable("Reservations_Tables", (string)null);
                });

            modelBuilder.Entity("restauracja_wpf.Models.Discount", b =>
                {
                    b.Property<int>("DiscountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DiscountId"));

                    b.Property<decimal>("DiscountMult")
                        .HasColumnType("decimal(3, 2)");

                    b.Property<string>("LevelName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<short>("OrderThreshold")
                        .HasColumnType("smallint");

                    b.HasKey("DiscountId");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Dish", b =>
                {
                    b.Property<int>("DishId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DishId"));

                    b.Property<bool>("Available")
                        .HasColumnType("bit");

                    b.Property<bool>("DishOfTheDay")
                        .HasColumnType("bit");

                    b.Property<bool>("Exclude")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<TimeSpan?>("TimeCalculated")
                        .HasColumnType("time");

                    b.Property<TimeSpan?>("TimeConstant")
                        .HasColumnType("time");

                    b.HasKey("DishId");

                    b.ToTable("Menu");
                });

            modelBuilder.Entity("restauracja_wpf.Models.DishOrder", b =>
                {
                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("DishId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDiscountable")
                        .HasColumnType("bit");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("OrderId", "DishId");

                    b.HasIndex("DishId");

                    b.ToTable("Menu_Orders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime");

                    b.Property<byte?>("DeliveryNumber")
                        .HasColumnType("tinyint");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime");

                    b.Property<int?>("RegularCustomerId")
                        .HasColumnType("int");

                    b.Property<int?>("ReservationId")
                        .HasColumnType("int");

                    b.Property<byte>("StatusId")
                        .HasColumnType("tinyint");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("RegularCustomerId");

                    b.HasIndex("ReservationId");

                    b.HasIndex("StatusId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.OrderStatus", b =>
                {
                    b.Property<byte>("StatusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<byte>("StatusId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("StatusId");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("restauracja_wpf.Models.RegularClient", b =>
                {
                    b.Property<int>("RegularClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegularClientId"));

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.HasKey("RegularClientId");

                    b.HasIndex("DiscountId");

                    b.ToTable("RegularClients");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Reservation", b =>
                {
                    b.Property<int>("ReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservationId"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime?>("ReservationDate")
                        .HasColumnType("datetime");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.HasKey("ReservationId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Restaurant", b =>
                {
                    b.Property<int>("RestaurantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RestaurantId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("RestaurantId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Table", b =>
                {
                    b.Property<int>("TableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TableId"));

                    b.Property<int>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<short>("Seats")
                        .HasColumnType("smallint");

                    b.Property<int>("TableNumber")
                        .HasColumnType("int");

                    b.HasKey("TableId");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("restauracja_wpf.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RestaurantId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OrderTable", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("restauracja_wpf.Models.Table", null)
                        .WithMany()
                        .HasForeignKey("TablesTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReservationTable", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Reservation", null)
                        .WithMany()
                        .HasForeignKey("ReservationsReservationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("restauracja_wpf.Models.Table", null)
                        .WithMany()
                        .HasForeignKey("TablesTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("restauracja_wpf.Models.DishOrder", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Dish", "Dish")
                        .WithMany("DishOrders")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("restauracja_wpf.Models.Order", "Order")
                        .WithMany("DishOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Order", b =>
                {
                    b.HasOne("restauracja_wpf.Models.RegularClient", "RegularCustomer")
                        .WithMany("Orders")
                        .HasForeignKey("RegularCustomerId");

                    b.HasOne("restauracja_wpf.Models.Reservation", "Reservation")
                        .WithMany("Orders")
                        .HasForeignKey("ReservationId");

                    b.HasOne("restauracja_wpf.Models.OrderStatus", "Status")
                        .WithMany("Orders")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("restauracja_wpf.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegularCustomer");

                    b.Navigation("Reservation");

                    b.Navigation("Status");

                    b.Navigation("User");
                });

            modelBuilder.Entity("restauracja_wpf.Models.RegularClient", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Discount", "Discount")
                        .WithMany("RegularCustomers")
                        .HasForeignKey("DiscountId");

                    b.Navigation("Discount");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Table", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Restaurant", "Restaurant")
                        .WithMany("Tables")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");
                });

            modelBuilder.Entity("restauracja_wpf.Models.User", b =>
                {
                    b.HasOne("restauracja_wpf.Models.Restaurant", "Restaurant")
                        .WithMany("Users")
                        .HasForeignKey("RestaurantId");

                    b.HasOne("restauracja_wpf.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Restaurant");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Discount", b =>
                {
                    b.Navigation("RegularCustomers");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Dish", b =>
                {
                    b.Navigation("DishOrders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Order", b =>
                {
                    b.Navigation("DishOrders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.OrderStatus", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.RegularClient", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Reservation", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Restaurant", b =>
                {
                    b.Navigation("Tables");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("restauracja_wpf.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("restauracja_wpf.Models.User", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
