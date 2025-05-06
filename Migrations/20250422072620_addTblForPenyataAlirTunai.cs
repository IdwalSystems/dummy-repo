using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblForPenyataAlirTunai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JKonfigPenyata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JKonfigPenyata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JKonfigPenyataBaris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bil = table.Column<int>(type: "int", nullable: false),
                    JKonfigPenyataId = table.Column<int>(type: "int", nullable: false),
                    EnKategoriTajuk = table.Column<int>(type: "int", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Susunan = table.Column<int>(type: "int", nullable: false),
                    IsFormula = table.Column<bool>(type: "bit", nullable: false),
                    EnKategoriJumlah = table.Column<int>(type: "int", nullable: false),
                    JumlahSusunanList = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JKonfigPenyataBaris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JKonfigPenyataBaris_JKonfigPenyata_JKonfigPenyataId",
                        column: x => x.JKonfigPenyataId,
                        principalTable: "JKonfigPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JKonfigPenyataBarisFormula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarisBil = table.Column<int>(type: "int", nullable: false),
                    JKonfigPenyataBarisId = table.Column<int>(type: "int", nullable: false),
                    EnJenisOperasi = table.Column<int>(type: "int", nullable: false),
                    IsPukal = table.Column<bool>(type: "bit", nullable: false),
                    EnJenisCartaList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsKecuali = table.Column<bool>(type: "bit", nullable: false),
                    KodList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetKodList = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JKonfigPenyataBarisFormula", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JKonfigPenyataBarisFormula_JKonfigPenyataBaris_JKonfigPenyataBarisId",
                        column: x => x.JKonfigPenyataBarisId,
                        principalTable: "JKonfigPenyataBaris",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JKonfigPenyataBaris_JKonfigPenyataId",
                table: "JKonfigPenyataBaris",
                column: "JKonfigPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_JKonfigPenyataBarisFormula_JKonfigPenyataBarisId",
                table: "JKonfigPenyataBarisFormula",
                column: "JKonfigPenyataBarisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JKonfigPenyataBarisFormula");

            migrationBuilder.DropTable(
                name: "JKonfigPenyataBaris");

            migrationBuilder.DropTable(
                name: "JKonfigPenyata");
        }
    }
}
