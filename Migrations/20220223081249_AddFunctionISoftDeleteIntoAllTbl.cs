using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFunctionISoftDeleteIntoAllTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkTunaiCV",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkTerima",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkPV",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkPOLaras",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkPO",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "FlBatal",
                table: "AkNotaMinta",
                newName: "FlHapus");

            migrationBuilder.RenameColumn(
                name: "Batal",
                table: "AkJurnal",
                newName: "FlHapus");

            migrationBuilder.AddColumn<int>(
                name: "FlHapus",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlCetak",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlHapus",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlPosting",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkTunaiRuncit",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkTunaiCV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkTerima",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkPOLaras",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkPO",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkJurnal",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "FlCetak",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "FlPosting",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkJurnal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkTunaiCV",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkTerima",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkPV",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkPOLaras",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkPO",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkNotaMinta",
                newName: "FlBatal");

            migrationBuilder.RenameColumn(
                name: "FlHapus",
                table: "AkJurnal",
                newName: "Batal");
        }
    }
}
