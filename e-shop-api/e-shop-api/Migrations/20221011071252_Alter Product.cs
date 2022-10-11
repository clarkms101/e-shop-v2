using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_shop_api.Migrations
{
    public partial class AlterProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 11, 15, 12, 51, 650, DateTimeKind.Local).AddTicks(612), new DateTime(2022, 10, 11, 15, 12, 51, 651, DateTimeKind.Local).AddTicks(1948) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 11, 14, 59, 7, 267, DateTimeKind.Local).AddTicks(1687), new DateTime(2022, 10, 11, 14, 59, 7, 268, DateTimeKind.Local).AddTicks(6524) });
        }
    }
}
