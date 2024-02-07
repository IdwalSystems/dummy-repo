using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblJKonfigPerubahanEkuiti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JKonfigPerubahanEkuiti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnLajurJadual = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_JKonfigPerubahanEkuiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JKonfigPerubahanEkuiti_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JKonfigPerubahanEkuitiBaris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKonfigPerubahanEkuitiId = table.Column<int>(type: "int", nullable: false),
                    EnBaris = table.Column<int>(type: "int", nullable: false),
                    EnJenisOperasi = table.Column<int>(type: "int", nullable: false),
                    IsPukal = table.Column<bool>(type: "bit", nullable: false),
                    EnJenisCartaList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsKecuali = table.Column<bool>(type: "bit", nullable: false),
                    KodList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetKodList = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JKonfigPerubahanEkuitiBaris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuiti_JKonfigPerubahanEkuitiId",
                        column: x => x.JKonfigPerubahanEkuitiId,
                        principalTable: "JKonfigPerubahanEkuiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JKonfigPerubahanEkuiti_JKWId",
                table: "JKonfigPerubahanEkuiti",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuitiId",
                table: "JKonfigPerubahanEkuitiBaris",
                column: "JKonfigPerubahanEkuitiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JKonfigPerubahanEkuitiBaris");

            migrationBuilder.DropTable(
                name: "JKonfigPerubahanEkuiti");
        }
    }
}
