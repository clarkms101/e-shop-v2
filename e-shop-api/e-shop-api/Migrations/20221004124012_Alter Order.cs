using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace e_shop_api.Migrations
{
    public partial class AlterOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Orders",
                type: "varchar(15)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 4, 20, 40, 12, 424, DateTimeKind.Local).AddTicks(8002), new DateTime(2022, 10, 4, 20, 40, 12, 425, DateTimeKind.Local).AddTicks(8817) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SerialNumber",
                table: "Orders",
                column: "SerialNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_SerialNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 6, 5, 13, 33, 44, 669, DateTimeKind.Local).AddTicks(1832), new DateTime(2022, 6, 5, 13, 33, 44, 670, DateTimeKind.Local).AddTicks(6259) });
        }
    }
}
