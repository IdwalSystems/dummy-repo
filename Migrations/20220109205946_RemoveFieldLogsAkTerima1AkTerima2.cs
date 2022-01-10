using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveFieldLogsAkTerima1AkTerima2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkTerima1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkTerima1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkTerima1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkTerima1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTerima2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkTerima2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkTerima2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkTerima2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTerima1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkTerima1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkTerima1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkTerima1",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
