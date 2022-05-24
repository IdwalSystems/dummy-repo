using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTableJProfilKategori : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuJurulatih",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "JProfilKategoriId",
                table: "SuJurulatih",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuAtlet",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateTable(
                name: "JProfilKategori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KadarGeran = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JProfilKategori", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JProfilKategoriId",
                table: "SuJurulatih",
                column: "JProfilKategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuProfilId",
                table: "AkPV",
                column: "SuProfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_SuProfil_SuProfilId",
                table: "AkPV",
                column: "SuProfilId",
                principalTable: "SuProfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuJurulatih_JProfilKategori_JProfilKategoriId",
                table: "SuJurulatih",
                column: "JProfilKategoriId",
                principalTable: "JProfilKategori",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_SuProfil_SuProfilId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_SuJurulatih_JProfilKategori_JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.DropTable(
                name: "JProfilKategori");

            migrationBuilder.DropIndex(
                name: "IX_SuJurulatih_JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_SuProfilId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "JProfilKategoriId",
                table: "SuJurulatih");

            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuJurulatih",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuAtlet",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
