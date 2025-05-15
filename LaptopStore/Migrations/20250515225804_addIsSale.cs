using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class addIsSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "Laptops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 1,
                column: "IsSold",
                value: false);

            migrationBuilder.UpdateData(
                table: "Laptops",
                keyColumn: "LaptopID",
                keyValue: 2,
                column: "IsSold",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "Laptops");
        }
    }
}
