using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldFlJenisJurnalFlKategoriPenerimaTblAkJurnal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlJenisJurnal",
                table: "AkJurnal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlKategoriPenerima",
                table: "AkJurnal",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlJenisJurnal",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "FlKategoriPenerima",
                table: "AkJurnal");
        }
    }
}
