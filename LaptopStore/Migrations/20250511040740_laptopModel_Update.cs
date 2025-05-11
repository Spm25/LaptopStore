using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class laptopModel_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Laptops",
                newName: "SellingPrice");

            migrationBuilder.AddColumn<int>(
                name: "BatteryHealth",
                table: "Laptops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "ImportPrice",
                table: "Laptops",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "OperatingSystem",
                table: "Laptops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "ScreenSize",
                table: "Laptops",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 1,
                columns: new[] { "BatteryHealth", "CPU", "Description", "GPU", "ImportPrice", "OperatingSystem", "ScreenSize", "SerialNumber", "Storage" },
                values: new object[] { 95, "Intel i7-11800H", "Dell high-end laptop, powerful and sleek", "NVIDIA RTX 3050", 1200f, "Windows 11", 15.6f, "SN12345678", "512GB SSD" });

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 2,
                columns: new[] { "BatteryHealth", "CPU", "Description", "GPU", "ImportPrice", "OperatingSystem", "ScreenSize", "SerialNumber", "Storage" },
                values: new object[] { 98, "Apple M1", "Apple MacBook Pro M1 chip, excellent battery life", "Apple M1 GPU", 1700f, "macOS Sonoma", 13.3f, "SN87654321", "1TB SSD" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatteryHealth",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "ImportPrice",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "OperatingSystem",
                table: "Laptops");

            migrationBuilder.DropColumn(
                name: "ScreenSize",
                table: "Laptops");

            migrationBuilder.RenameColumn(
                name: "SellingPrice",
                table: "Laptops",
                newName: "Price");

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 1,
                columns: new[] { "CPU", "Description", "GPU", "SerialNumber", "Storage" },
                values: new object[] { "Intel i7", "Dell high-end laptop", "RTX 3050", "SNDXPS001", "512GB" });

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 2,
                columns: new[] { "CPU", "Description", "GPU", "SerialNumber", "Storage" },
                values: new object[] { "M1", "Apple MacBook Pro M1", "M1 GPU", "SNMBP001", "1TB" });
        }
    }
}
