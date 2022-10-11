using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace e_shop_api.Migrations
{
    public partial class AddCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "varchar(50)", nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", nullable: true),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 11, 14, 59, 7, 267, DateTimeKind.Local).AddTicks(1687), new DateTime(2022, 10, 11, 14, 59, 7, 268, DateTimeKind.Local).AddTicks(6524) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "LastModificationTime" },
                values: new object[] { new DateTime(2022, 10, 5, 15, 2, 16, 473, DateTimeKind.Local).AddTicks(6189), new DateTime(2022, 10, 5, 15, 2, 16, 474, DateTimeKind.Local).AddTicks(8053) });
        }
    }
}
