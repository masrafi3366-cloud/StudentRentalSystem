using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "Email", "Password" },
                values: new object[] { 1, "admin@gmail.com", "Admin@123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: 1);
        }
    }
}
