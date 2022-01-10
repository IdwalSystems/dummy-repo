using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveLogFieldsTblAkBelian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkBelian2");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkBelian2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkBelian2");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkBelian2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkBelian1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkBelian1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkBelian1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkBelian1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBelian2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkBelian2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkBelian2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkBelian2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBelian1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkBelian1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkBelian1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkBelian1",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
