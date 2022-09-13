using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addInterfaceICancel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "AkPOLaras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "AkPOLaras",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "AkPO",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "AkPO",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "AkInden",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "AkInden",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlBatal",
                table: "AkBelian",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarBatal",
                table: "AkBelian",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "AkInden");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "AkInden");

            migrationBuilder.DropColumn(
                name: "FlBatal",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "TarBatal",
                table: "AkBelian");
        }
    }
}
