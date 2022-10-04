using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldFlTunaiTarTunaiTblAkPVAkJurnalAkTerima2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlTunai",
                table: "AkTerima2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarTunai",
                table: "AkTerima2",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlTunai",
                table: "AkPVGanda",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarTunai",
                table: "AkPVGanda",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlTunai",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarTunai",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlTunai",
                table: "AkJurnal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarTunai",
                table: "AkJurnal",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlTunai",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "TarTunai",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "FlTunai",
                table: "AkPVGanda");

            migrationBuilder.DropColumn(
                name: "TarTunai",
                table: "AkPVGanda");

            migrationBuilder.DropColumn(
                name: "FlTunai",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarTunai",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "FlTunai",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "TarTunai",
                table: "AkJurnal");
        }
    }
}
