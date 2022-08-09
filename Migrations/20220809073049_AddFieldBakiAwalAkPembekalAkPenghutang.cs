using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldBakiAwalAkPembekalAkPenghutang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BakiAwal",
                table: "AkPenghutang",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BakiAwal",
                table: "AkPembekal",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BakiAwal",
                table: "AkPenghutang");

            migrationBuilder.DropColumn(
                name: "BakiAwal",
                table: "AkPembekal");
        }
    }
}
