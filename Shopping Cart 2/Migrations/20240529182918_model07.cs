using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_Cart_2.Migrations
{
    /// <inheritdoc />
    public partial class model07 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "items",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "items");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "items");
        }
    }
}
