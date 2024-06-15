using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Cart_2.Migrations
{
    /// <inheritdoc />
    public partial class model05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
