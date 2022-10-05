using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_shop_api.Migrations
{
    public partial class AlterOrderV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Orders",
                type: "varchar(20)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 5, 15, 2, 16, 473, DateTimeKind.Local).AddTicks(6189), new DateTime(2022, 10, 5, 15, 2, 16, 474, DateTimeKind.Local).AddTicks(8053) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 5, 11, 25, 50, 379, DateTimeKind.Local).AddTicks(9159), new DateTime(2022, 10, 5, 11, 25, 50, 381, DateTimeKind.Local).AddTicks(277) });
        }
    }
}
