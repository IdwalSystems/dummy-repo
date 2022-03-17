using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldFlJenisTerimaFlKategoriPenerimaIntoTblAkTerima : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlJenisTerima",
                table: "AkTerima",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlKategoriPembayar",
                table: "AkTerima",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlJenisTerima",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "FlKategoriPembayar",
                table: "AkTerima");
        }
    }
}
