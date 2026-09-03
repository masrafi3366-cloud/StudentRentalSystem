using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddItemRentalStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Items");
        }
    }
}
