using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnStatusToRental : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "Rentals");
        }
    }
}
