using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaptopStore.Migrations
{
    /// <inheritdoc />
    public partial class addRoleForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "123456", "staff" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: 2,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "password", "employee" });
        }
    }
}
