using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddRentalReturnDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "AdminId",
                keyValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Rentals",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Rentals");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "AdminId", "Email", "Password" },
                values: new object[] { 1, "admin@gmail.com", "Admin@123" });
        }
    }
}
