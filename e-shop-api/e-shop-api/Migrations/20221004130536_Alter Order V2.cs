using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_shop_api.Migrations
{
    public partial class AlterOrderV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Orders",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 4, 21, 5, 36, 363, DateTimeKind.Local).AddTicks(2186), new DateTime(2022, 10, 4, 21, 5, 36, 364, DateTimeKind.Local).AddTicks(3297) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Orders",
                type: "varchar(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 4, 20, 40, 12, 424, DateTimeKind.Local).AddTicks(8002), new DateTime(2022, 10, 4, 20, 40, 12, 425, DateTimeKind.Local).AddTicks(8817) });
        }
    }
}
