using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Cart_2.Migrations
{
    /// <inheritdoc />
    public partial class m11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAverageRate",
                table: "Ratings");

            migrationBuilder.AddColumn<double>(
                name: "ProductAverageRate",
                table: "items",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAverageRate",
                table: "items");

            migrationBuilder.AddColumn<double>(
                name: "ProductAverageRate",
                table: "Ratings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
