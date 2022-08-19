using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebshopApi.Data.Migrations
{
    public partial class AddSubscriptionFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubscription",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscription",
                table: "Products");
        }
    }
}
