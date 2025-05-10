using Microsoft.EntityFrameworkCore;
using LaptopStore.Models;

namespace LaptopStore.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Laptop> Laptops { get; set; }
    public DbSet<LaptopScreen> LaptopScreens { get; set; }
    public DbSet<LaptopBattery> LaptopBatteries { get; set; }
    public DbSet<LaptopCharger> LaptopChargers { get; set; }
    public DbSet<RAM> RAMs { get; set; }
    public DbSet<StorageDevice> StorageDevices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerID);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany()
            .HasForeignKey(o => o.UserID);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany()
            .HasForeignKey(od => od.OrderID);

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { UserID = 1, UserName = "admin", Password = "123456", Role = 1 },
            new User { UserID = 2, UserName = "employee", Password = "password", Role = 2 }
        );

        // Seed Customers
        modelBuilder.Entity<Customer>().HasData(
            new Customer { CustomerID = 1, FullName = "Nguyễn Văn A", Phone = "0123456789", Email = "a@gmail.com", Address = "Hà Nội" },
            new Customer { CustomerID = 2, FullName = "Trần Thị B", Phone = "0987654321", Email = "b@gmail.com", Address = "Hồ Chí Minh" }
        );

        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { OrderID = 1, OrderDate = new DateTime(2025, 2, 17), Paid = true, CustomerID = 1, UserID = 1 },
            new Order { OrderID = 2, OrderDate = new DateTime(2025, 2, 16), Paid = false, CustomerID = 2, UserID = 2 }
        );

        // Seed Order Details
        modelBuilder.Entity<OrderDetail>().HasData(
            new OrderDetail { OrderDetailID = 1, OrderID = 1, ProductID = 1, ProductType = ProductType.Laptop, Quantity = 1, UnitPrice = 1500.0f, WarrantyPeriod = "12 tháng" },
            new OrderDetail { OrderDetailID = 2, OrderID = 2, ProductID = 1, ProductType = ProductType.RAM, Quantity = 2, UnitPrice = 45.0f, WarrantyPeriod = "6 tháng" }
        );

        // Seed Laptops
        modelBuilder.Entity<Laptop>().HasData(
            new Laptop { LaptopID = 1, Brand = "Dell", Model = "XPS 15", CPU = "Intel i7", RAM = "16GB", StorageType = "SSD", StorageSize = "512GB", GPU = "RTX 3050", Price = 1500, Description = "Dell high-end laptop", ImageURL = "dell_xps.jpg" },
            new Laptop { LaptopID = 2, Brand = "Apple", Model = "MacBook Pro", CPU = "M1", RAM = "16GB", StorageType = "SSD", StorageSize = "1TB", GPU = "M1 GPU", Price = 2000, Description = "Apple MacBook Pro M1", ImageURL = "macbook_pro.jpg" }
        );

        // Seed RAM
        modelBuilder.Entity<RAM>().HasData(
            new RAM { RAMID = 1, Capacity = 16, Speed = 3200, Type = "DDR4", Quantity = 20, Quality = "Chính hãng" },
            new RAM { RAMID = 2, Capacity = 8, Speed = 2666, Type = "DDR3", Quantity = 10, Quality = "Hàng nhái" }
        );

        // Seed Storage Devices
        modelBuilder.Entity<StorageDevice>().HasData(
            new StorageDevice { StorageID = 1, Type = "SSD", Capacity = "1TB", Quantity = 15, Quality = "Chính hãng" },
            new StorageDevice { StorageID = 2, Type = "HDD", Capacity = "2TB", Quantity = 10, Quality = "Hàng nhái" }
        );

        // Seed Laptop Screens
        modelBuilder.Entity<LaptopScreen>().HasData(
            new LaptopScreen { ScreenID = 1, Resolution = "1920x1080", ScreenType = "IPS", UsedStatus = "New", Repaired = 0, Quantity = 10, Quality = "Chính hãng" },
            new LaptopScreen { ScreenID = 2, Resolution = "2560x1440", ScreenType = "OLED", UsedStatus = "Used", Repaired = 1, Quantity = 5, Quality = "Hàng om" }
        );

        // Seed Laptop Batteries
        modelBuilder.Entity<LaptopBattery>().HasData(
            new LaptopBattery { BatteryID = 1, LaptopModel = "XPS 15", Capacity = "86Wh", UsedStatus = 0, Type = "Lithium-ion", Quantity = 20, Quality = "Chính hãng" },
            new LaptopBattery { BatteryID = 2, LaptopModel = "MacBook Pro", Capacity = "100Wh", UsedStatus = 1, Type = "Lithium-ion", Quantity = 8, Quality = "Hàng om" }
        );

        // Seed Laptop Chargers
        modelBuilder.Entity<LaptopCharger>().HasData(
            new LaptopCharger { ChargerID = 1, Wattage = 130, Connector = "USB-C", Quantity = 15, Quality = "Chính hãng" },
            new LaptopCharger { ChargerID = 2, Wattage = 65, Connector = "Barrel", Quantity = 10, Quality = "Hàng om" }
        );

    }
}
