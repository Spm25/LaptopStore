using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class addServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ServiceID);
                });

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailID",
                keyValue: 2,
                column: "ProductType",
                value: 2);

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "ServiceID", "Description", "Price", "ServiceName" },
                values: new object[,]
                {
                    { 1, "Vệ sinh quạt tản nhiệt, bôi keo", 100000f, "Vệ sinh máy" },
                    { 2, "Phí nâng cấp RAM cho máy khách", 150000f, "Nâng cấp RAM" },
                    { 3, "Keo tản nhiệt chất lượng cao", 80000f, "Thay keo tản nhiệt" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.UpdateData(
                table: "OrderDetails",
                keyColumn: "OrderDetailID",
                keyValue: 2,
                column: "ProductType",
                value: 1);
        }
    }
}
