using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldsTblAkNotaMinta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoPO",
                table: "AkNotaMinta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhPosting",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoPO",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "TarikhPosting",
                table: "AkNotaMinta");
        }
    }
}
