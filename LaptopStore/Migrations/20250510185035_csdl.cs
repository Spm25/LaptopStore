using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class csdl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "LaptopBatteries",
                columns: table => new
                {
                    BatteryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LaptopModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedStatus = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopBatteries", x => x.BatteryID);
                });

            migrationBuilder.CreateTable(
                name: "LaptopChargers",
                columns: table => new
                {
                    ChargerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Wattage = table.Column<int>(type: "int", nullable: false),
                    Connector = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopChargers", x => x.ChargerID);
                });

            migrationBuilder.CreateTable(
                name: "Laptops",
                columns: table => new
                {
                    LaptopID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CPU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StorageSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GPU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptops", x => x.LaptopID);
                });

            migrationBuilder.CreateTable(
                name: "LaptopScreens",
                columns: table => new
                {
                    ScreenID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsedStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Repaired = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopScreens", x => x.ScreenID);
                });

            migrationBuilder.CreateTable(
                name: "RAMs",
                columns: table => new
                {
                    RAMID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMs", x => x.RAMID);
                });

            migrationBuilder.CreateTable(
                name: "StorageDevices",
                columns: table => new
                {
                    StorageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capacity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageDevices", x => x.StorageID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<float>(type: "real", nullable: false),
                    WarrantyPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailID);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerID", "Address", "Email", "FullName", "Phone" },
                values: new object[,]
                {
                    { 1, "Hà Nội", "a@gmail.com", "Nguyễn Văn A", "0123456789" },
                    { 2, "Hồ Chí Minh", "b@gmail.com", "Trần Thị B", "0987654321" }
                });

            migrationBuilder.InsertData(
                table: "LaptopBatteries",
                columns: new[] { "BatteryID", "Capacity", "LaptopModel", "Quality", "Quantity", "Type", "UsedStatus" },
                values: new object[,]
                {
                    { 1, "86Wh", "XPS 15", "Chính hãng", 20, "Lithium-ion", 0 },
                    { 2, "100Wh", "MacBook Pro", "Hàng om", 8, "Lithium-ion", 1 }
                });

            migrationBuilder.InsertData(
                table: "LaptopChargers",
                columns: new[] { "ChargerID", "Connector", "Quality", "Quantity", "Wattage" },
                values: new object[,]
                {
                    { 1, "USB-C", "Chính hãng", 15, 130 },
                    { 2, "Barrel", "Hàng om", 10, 65 }
                });

            migrationBuilder.InsertData(
                table: "LaptopScreens",
                columns: new[] { "ScreenID", "Quality", "Quantity", "Repaired", "Resolution", "ScreenType", "UsedStatus" },
                values: new object[,]
                {
                    { 1, "Chính hãng", 10, 0, "1920x1080", "IPS", "New" },
                    { 2, "Hàng om", 5, 1, "2560x1440", "OLED", "Used" }
                });

            migrationBuilder.InsertData(
                table: "Laptops",
                columns: new[] { "LaptopID", "Brand", "CPU", "Description", "GPU", "ImageURL", "Model", "Price", "RAM", "StorageSize", "StorageType" },
                values: new object[,]
                {
                    { 1, "Dell", "Intel i7", "Dell high-end laptop", "RTX 3050", "dell_xps.jpg", "XPS 15", 1500f, "16GB", "512GB", "SSD" },
                    { 2, "Apple", "M1", "Apple MacBook Pro M1", "M1 GPU", "macbook_pro.jpg", "MacBook Pro", 2000f, "16GB", "1TB", "SSD" }
                });

            migrationBuilder.InsertData(
                table: "RAMs",
                columns: new[] { "RAMID", "Capacity", "Quality", "Quantity", "Speed", "Type" },
                values: new object[,]
                {
                    { 1, 16, "Chính hãng", 20, 3200, "DDR4" },
                    { 2, 8, "Hàng nhái", 10, 2666, "DDR3" }
                });

            migrationBuilder.InsertData(
                table: "StorageDevices",
                columns: new[] { "StorageID", "Capacity", "Quality", "Quantity", "Type" },
                values: new object[,]
                {
                    { 1, "1TB", "Chính hãng", 15, "SSD" },
                    { 2, "2TB", "Hàng nhái", 10, "HDD" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Password", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "123456", 1, "admin" },
                    { 2, "password", 2, "employee" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderID", "CustomerID", "OrderDate", "Paid", "UserID" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 1 },
                    { 2, 2, new DateTime(2025, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2 }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailID", "OrderID", "ProductID", "ProductType", "Quantity", "UnitPrice", "WarrantyPeriod" },
                values: new object[,]
                {
                    { 1, 1, 1, 0, 1, 1500f, "12 tháng" },
                    { 2, 2, 1, 1, 2, 45f, "6 tháng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderID",
                table: "OrderDetails",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerID",
                table: "Orders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaptopBatteries");

            migrationBuilder.DropTable(
                name: "LaptopChargers");

            migrationBuilder.DropTable(
                name: "Laptops");

            migrationBuilder.DropTable(
                name: "LaptopScreens");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "RAMs");

            migrationBuilder.DropTable(
                name: "StorageDevices");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
