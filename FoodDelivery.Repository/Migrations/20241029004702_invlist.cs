using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Repository.Migrations
{
    /// <inheritdoc />
    public partial class invlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_WarehouseId",
                table: "Products",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warehouses_WarehouseId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_WarehouseId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Products");
        }
    }
}
