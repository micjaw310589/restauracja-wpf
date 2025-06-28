using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restauracja_wpf.Migrations
{
    /// <inheritdoc />
    public partial class DishOrder_Quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Menu_Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Menu_Orders");
        }
    }
}
