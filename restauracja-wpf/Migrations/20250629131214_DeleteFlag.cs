using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace restauracja_wpf.Migrations
{
    /// <inheritdoc />
    public partial class DeleteFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Tables",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Status",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "RegularClients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Menu_Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Menu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "RegularClients");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Menu_Orders");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "Discounts");
        }
    }
}
