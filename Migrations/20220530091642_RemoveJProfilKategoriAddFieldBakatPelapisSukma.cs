using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveJProfilKategoriAddFieldBakatPelapisSukma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuJurulatih_JProfilKategori_JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.DropIndex(
                name: "IX_SuJurulatih_JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.AddColumn<bool>(
                name: "IsJSMBakat",
                table: "SuJurulatih",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsJSMPelapis",
                table: "SuJurulatih",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSukma",
                table: "SuJurulatih",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJSMBakat",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "IsJSMPelapis",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "IsSukma",
                table: "SuJurulatih");

            migrationBuilder.AddColumn<int>(
                name: "JProfilKategoriId",
                table: "SuJurulatih",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JProfilKategoriId",
                table: "SuJurulatih",
                column: "JProfilKategoriId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuJurulatih_JProfilKategori_JProfilKategoriId",
                table: "SuJurulatih",
                column: "JProfilKategoriId",
                principalTable: "JProfilKategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
