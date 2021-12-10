using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class model_jurnal_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoRujukan",
                table: "AkJurnal1");

            migrationBuilder.AddColumn<string>(
                name: "KodEFT",
                table: "JBank",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KodEFT",
                table: "JBank");

            migrationBuilder.AddColumn<string>(
                name: "NoRujukan",
                table: "AkJurnal1",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
