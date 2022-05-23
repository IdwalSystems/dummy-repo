using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldTarikhPostingInTableSuProfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhPosting",
                table: "SuProfil",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuProfilId",
                table: "AkPV",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarikhPosting",
                table: "SuProfil");

            migrationBuilder.DropColumn(
                name: "SuProfilId",
                table: "AkPV");
        }
    }
}
