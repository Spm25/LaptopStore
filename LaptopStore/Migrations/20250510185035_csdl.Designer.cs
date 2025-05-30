﻿// <auto-generated />
using System;
using LaptopStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LaptopStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250510185035_csdl")]
    partial class csdl
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LaptopStore.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            CustomerID = 1,
                            Address = "Hà Nội",
                            Email = "a@gmail.com",
                            FullName = "Nguyễn Văn A",
                            Phone = "0123456789"
                        },
                        new
                        {
                            CustomerID = 2,
                            Address = "Hồ Chí Minh",
                            Email = "b@gmail.com",
                            FullName = "Trần Thị B",
                            Phone = "0987654321"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.Laptop", b =>
                {
                    b.Property<int>("LaptopID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LaptopID"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CPU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GPU")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("RAM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StorageSize")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StorageType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LaptopID");

                    b.ToTable("Laptops");

                    b.HasData(
                        new
                        {
                            LaptopID = 1,
                            Brand = "Dell",
                            CPU = "Intel i7",
                            Description = "Dell high-end laptop",
                            GPU = "RTX 3050",
                            ImageURL = "dell_xps.jpg",
                            Model = "XPS 15",
                            Price = 1500f,
                            RAM = "16GB",
                            StorageSize = "512GB",
                            StorageType = "SSD"
                        },
                        new
                        {
                            LaptopID = 2,
                            Brand = "Apple",
                            CPU = "M1",
                            Description = "Apple MacBook Pro M1",
                            GPU = "M1 GPU",
                            ImageURL = "macbook_pro.jpg",
                            Model = "MacBook Pro",
                            Price = 2000f,
                            RAM = "16GB",
                            StorageSize = "1TB",
                            StorageType = "SSD"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.LaptopBattery", b =>
                {
                    b.Property<int>("BatteryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BatteryID"));

                    b.Property<string>("Capacity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LaptopModel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsedStatus")
                        .HasColumnType("int");

                    b.HasKey("BatteryID");

                    b.ToTable("LaptopBatteries");

                    b.HasData(
                        new
                        {
                            BatteryID = 1,
                            Capacity = "86Wh",
                            LaptopModel = "XPS 15",
                            Quality = "Chính hãng",
                            Quantity = 20,
                            Type = "Lithium-ion",
                            UsedStatus = 0
                        },
                        new
                        {
                            BatteryID = 2,
                            Capacity = "100Wh",
                            LaptopModel = "MacBook Pro",
                            Quality = "Hàng om",
                            Quantity = 8,
                            Type = "Lithium-ion",
                            UsedStatus = 1
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.LaptopCharger", b =>
                {
                    b.Property<int>("ChargerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChargerID"));

                    b.Property<string>("Connector")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Wattage")
                        .HasColumnType("int");

                    b.HasKey("ChargerID");

                    b.ToTable("LaptopChargers");

                    b.HasData(
                        new
                        {
                            ChargerID = 1,
                            Connector = "USB-C",
                            Quality = "Chính hãng",
                            Quantity = 15,
                            Wattage = 130
                        },
                        new
                        {
                            ChargerID = 2,
                            Connector = "Barrel",
                            Quality = "Hàng om",
                            Quantity = 10,
                            Wattage = 65
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.LaptopScreen", b =>
                {
                    b.Property<int>("ScreenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScreenID"));

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Repaired")
                        .HasColumnType("int");

                    b.Property<string>("Resolution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScreenType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsedStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScreenID");

                    b.ToTable("LaptopScreens");

                    b.HasData(
                        new
                        {
                            ScreenID = 1,
                            Quality = "Chính hãng",
                            Quantity = 10,
                            Repaired = 0,
                            Resolution = "1920x1080",
                            ScreenType = "IPS",
                            UsedStatus = "New"
                        },
                        new
                        {
                            ScreenID = 2,
                            Quality = "Hàng om",
                            Quantity = 5,
                            Repaired = 1,
                            Resolution = "2560x1440",
                            ScreenType = "OLED",
                            UsedStatus = "Used"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderID"));

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Paid")
                        .HasColumnType("bit");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("OrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            OrderID = 1,
                            CustomerID = 1,
                            OrderDate = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Paid = true,
                            UserID = 1
                        },
                        new
                        {
                            OrderID = 2,
                            CustomerID = 2,
                            OrderDate = new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Paid = false,
                            UserID = 2
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailID"));

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("ProductType")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<float>("UnitPrice")
                        .HasColumnType("real");

                    b.Property<string>("WarrantyPeriod")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderDetailID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderDetails");

                    b.HasData(
                        new
                        {
                            OrderDetailID = 1,
                            OrderID = 1,
                            ProductID = 1,
                            ProductType = 0,
                            Quantity = 1,
                            UnitPrice = 1500f,
                            WarrantyPeriod = "12 tháng"
                        },
                        new
                        {
                            OrderDetailID = 2,
                            OrderID = 2,
                            ProductID = 1,
                            ProductType = 1,
                            Quantity = 2,
                            UnitPrice = 45f,
                            WarrantyPeriod = "6 tháng"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.RAM", b =>
                {
                    b.Property<int>("RAMID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RAMID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RAMID");

                    b.ToTable("RAMs");

                    b.HasData(
                        new
                        {
                            RAMID = 1,
                            Capacity = 16,
                            Quality = "Chính hãng",
                            Quantity = 20,
                            Speed = 3200,
                            Type = "DDR4"
                        },
                        new
                        {
                            RAMID = 2,
                            Capacity = 8,
                            Quality = "Hàng nhái",
                            Quantity = 10,
                            Speed = 2666,
                            Type = "DDR3"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.StorageDevice", b =>
                {
                    b.Property<int>("StorageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StorageID"));

                    b.Property<string>("Capacity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StorageID");

                    b.ToTable("StorageDevices");

                    b.HasData(
                        new
                        {
                            StorageID = 1,
                            Capacity = "1TB",
                            Quality = "Chính hãng",
                            Quantity = 15,
                            Type = "SSD"
                        },
                        new
                        {
                            StorageID = 2,
                            Capacity = "2TB",
                            Quality = "Hàng nhái",
                            Quantity = 10,
                            Type = "HDD"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserID = 1,
                            Password = "123456",
                            Role = 1,
                            UserName = "admin"
                        },
                        new
                        {
                            UserID = 2,
                            Password = "password",
                            Role = 2,
                            UserName = "employee"
                        });
                });

            modelBuilder.Entity("LaptopStore.Models.Order", b =>
                {
                    b.HasOne("LaptopStore.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LaptopStore.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LaptopStore.Models.OrderDetail", b =>
                {
                    b.HasOne("LaptopStore.Models.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });
#pragma warning restore 612, 618
        }
    }
}
