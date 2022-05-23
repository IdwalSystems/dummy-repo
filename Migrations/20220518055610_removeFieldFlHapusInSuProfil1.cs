using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class removeFieldFlHapusInSuProfil1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "SuProfil1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlHapus",
                table: "SuProfil1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "SuProfil1",
                type: "datetime2",
                nullable: true);
        }
    }
}
