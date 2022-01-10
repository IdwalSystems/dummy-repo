using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveLogFieldsTblAkPV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkPV2");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkPV2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkPV2");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkPV2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkPV1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkPV1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkPV1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkPV1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPV2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkPV2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkPV2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkPV2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPV1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkPV1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkPV1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkPV1",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
