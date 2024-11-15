using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Repository.Migrations
{
    /// <inheritdoc />
    public partial class uom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitOfMeasurements_UnitOfMeasurementId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitOfMeasurements",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UnitOfMeasurements");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UnitOfMeasurements");

            migrationBuilder.RenameTable(
                name: "UnitOfMeasurements",
                newName: "UnitsOfMeasurement");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "ProductTypes",
                newName: "TypeID");

            migrationBuilder.RenameColumn(
                name: "UnitOfMeasurementId",
                table: "UnitsOfMeasurement",
                newName: "UoMID");

            migrationBuilder.AddColumn<string>(
                name: "TypeName",
                table: "ProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UoMName",
                table: "UnitsOfMeasurement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitsOfMeasurement",
                table: "UnitsOfMeasurement",
                column: "UoMID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitsOfMeasurement_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId",
                principalTable: "UnitsOfMeasurement",
                principalColumn: "UoMID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitsOfMeasurement_UnitOfMeasurementId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UnitsOfMeasurement",
                table: "UnitsOfMeasurement");

            migrationBuilder.DropColumn(
                name: "TypeName",
                table: "ProductTypes");

            migrationBuilder.DropColumn(
                name: "UoMName",
                table: "UnitsOfMeasurement");

            migrationBuilder.RenameTable(
                name: "UnitsOfMeasurement",
                newName: "UnitOfMeasurements");

            migrationBuilder.RenameColumn(
                name: "TypeID",
                table: "ProductTypes",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "UoMID",
                table: "UnitOfMeasurements",
                newName: "UnitOfMeasurementId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProductTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductTypes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ProductTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                table: "UnitOfMeasurements",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UnitOfMeasurements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UnitOfMeasurements",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UnitOfMeasurements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UnitOfMeasurements",
                table: "UnitOfMeasurements",
                column: "UnitOfMeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitOfMeasurements_UnitOfMeasurementId",
                table: "Products",
                column: "UnitOfMeasurementId",
                principalTable: "UnitOfMeasurements",
                principalColumn: "UnitOfMeasurementId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
