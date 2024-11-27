using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Repository.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Suppliers",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                   Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                   Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                   Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                   TaxID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                   CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Suppliers", x => x.Id);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Suppliers");
        }
    }
}
