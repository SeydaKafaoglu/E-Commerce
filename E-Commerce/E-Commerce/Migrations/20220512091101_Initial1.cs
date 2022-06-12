using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SellerName",
                table: "Sellers",
                type: "nchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(50)");

            migrationBuilder.AddColumn<bool>(
                name: "Banned",
                table: "Sellers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SellerEMail",
                table: "Sellers",
                type: "char(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SellerPassword",
                table: "Sellers",
                type: "char(64)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SellerPhone",
                table: "Sellers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Banned",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerEMail",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerPassword",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "SellerPhone",
                table: "Sellers");

            migrationBuilder.AlterColumn<string>(
                name: "SellerName",
                table: "Sellers",
                type: "char(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)");
        }
    }
}
