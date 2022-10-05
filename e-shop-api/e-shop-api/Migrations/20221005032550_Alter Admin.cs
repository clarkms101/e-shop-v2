using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_shop_api.Migrations
{
    public partial class AlterAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Permission",
                table: "Admins",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 5, 11, 25, 50, 379, DateTimeKind.Local).AddTicks(9159), new DateTime(2022, 10, 5, 11, 25, 50, 381, DateTimeKind.Local).AddTicks(277) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "Admins");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 4, 21, 5, 36, 363, DateTimeKind.Local).AddTicks(2186), new DateTime(2022, 10, 4, 21, 5, 36, 364, DateTimeKind.Local).AddTicks(3297) });
        }
    }
}
