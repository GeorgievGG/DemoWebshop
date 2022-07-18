using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebshopApi.Data.Migrations
{
    public partial class RefactorOrderLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Paid",
                table: "Orders",
                newName: "Confirmed");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Confirmed",
                table: "Orders",
                newName: "Paid");
        }
    }
}
