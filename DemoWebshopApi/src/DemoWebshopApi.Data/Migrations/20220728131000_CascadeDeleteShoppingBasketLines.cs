using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoWebshopApi.Data.Migrations
{
    public partial class CascadeDeleteShoppingBasketLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingBasketLines_ShoppingBaskets_BasketId",
                table: "ShoppingBasketLines");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingBasketLines_ShoppingBaskets_BasketId",
                table: "ShoppingBasketLines",
                column: "BasketId",
                principalTable: "ShoppingBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingBasketLines_ShoppingBaskets_BasketId",
                table: "ShoppingBasketLines");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingBasketLines_ShoppingBaskets_BasketId",
                table: "ShoppingBasketLines",
                column: "BasketId",
                principalTable: "ShoppingBaskets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
