using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaptopModel_AddSerialNumber_RemoveStorageType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StorageType",
                table: "Laptops",
                newName: "Storage");

            migrationBuilder.RenameColumn(
                name: "StorageSize",
                table: "Laptops",
                newName: "SerialNumber");

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 1,
                columns: new[] { "SerialNumber", "Storage" },
                values: new object[] { "SNDXPS001", "512GB" });

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 2,
                columns: new[] { "SerialNumber", "Storage" },
                values: new object[] { "SNMBP001", "1TB" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Storage",
                table: "Laptops",
                newName: "StorageType");

            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Laptops",
                newName: "StorageSize");

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 1,
                columns: new[] { "StorageSize", "StorageType" },
                values: new object[] { "512GB", "SSD" });

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 2,
                columns: new[] { "StorageSize", "StorageType" },
                values: new object[] { "1TB", "SSD" });
        }
    }
}
