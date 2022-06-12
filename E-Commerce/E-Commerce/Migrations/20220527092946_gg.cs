using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    public partial class gg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_ProductId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId1",
                table: "Products",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductId1",
                table: "Products",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_ProductId1",
                table: "Products",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "ProductId");
        }
    }
}
