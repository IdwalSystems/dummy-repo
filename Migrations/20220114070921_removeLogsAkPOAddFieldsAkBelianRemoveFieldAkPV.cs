using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class removeLogsAkPOAddFieldsAkBelianRemoveFieldAkPV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarikhTerima",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkPO2");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkPO2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkPO2");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkPO2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkPO1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkPO1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkPO1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkPO1");

            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhKewanganTerima",
                table: "AkBelian",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhTerima",
                table: "AkBelian",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarikhKewanganTerima",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "TarikhTerima",
                table: "AkBelian");

            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhTerima",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPO2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkPO2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkPO2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkPO2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPO1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkPO1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkPO1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkPO1",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
