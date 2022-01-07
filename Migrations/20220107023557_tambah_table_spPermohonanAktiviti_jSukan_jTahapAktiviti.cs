using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class tambah_table_spPermohonanAktiviti_jSukan_jTahapAktiviti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlCetak",
                table: "AkPO",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JSukan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JSukan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JTahapAktiviti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JTahapAktiviti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpPermohonanAktiviti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ppn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Penyertaan = table.Column<int>(type: "int", nullable: false),
                    Pertandingan = table.Column<int>(type: "int", nullable: false),
                    Pengelolaan = table.Column<int>(type: "int", nullable: false),
                    ProgramBinaan = table.Column<int>(type: "int", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktiviti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tempat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JTahapId = table.Column<int>(type: "int", nullable: false),
                    Penyedia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarSedia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JumKeseluruhan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Penyokong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusSokong = table.Column<int>(type: "int", nullable: false),
                    TarSokong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JumSokong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Pelulus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusLulus = table.Column<int>(type: "int", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JumLulus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    JTahapAktivitiId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPermohonanAktiviti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti_JTahapAktiviti_JTahapAktivitiId",
                        column: x => x.JTahapAktivitiId,
                        principalTable: "JTahapAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpPermohonanAktiviti1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kadar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bil = table.Column<int>(type: "int", nullable: false),
                    Bln = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpPermohonanAktivitiId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPermohonanAktiviti1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti1_SpPermohonanAktiviti_SpPermohonanAktivitiId",
                        column: x => x.SpPermohonanAktivitiId,
                        principalTable: "SpPermohonanAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpPermohonanAktiviti2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BilAtlL = table.Column<int>(type: "int", nullable: false),
                    BilJulL = table.Column<int>(type: "int", nullable: false),
                    BilPegL = table.Column<int>(type: "int", nullable: false),
                    BilTekL = table.Column<int>(type: "int", nullable: false),
                    BilUruL = table.Column<int>(type: "int", nullable: false),
                    BilAtlP = table.Column<int>(type: "int", nullable: false),
                    BilJulP = table.Column<int>(type: "int", nullable: false),
                    BilPegP = table.Column<int>(type: "int", nullable: false),
                    BilTekP = table.Column<int>(type: "int", nullable: false),
                    BilUruP = table.Column<int>(type: "int", nullable: false),
                    JumL = table.Column<int>(type: "int", nullable: false),
                    JumP = table.Column<int>(type: "int", nullable: false),
                    JumAtl = table.Column<int>(type: "int", nullable: false),
                    JumJul = table.Column<int>(type: "int", nullable: false),
                    JumPeg = table.Column<int>(type: "int", nullable: false),
                    JumTek = table.Column<int>(type: "int", nullable: false),
                    JumUru = table.Column<int>(type: "int", nullable: false),
                    SpPermohonanAktivitiId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPermohonanAktiviti2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti2_SpPermohonanAktiviti_SpPermohonanAktivitiId",
                        column: x => x.SpPermohonanAktivitiId,
                        principalTable: "SpPermohonanAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti_JNegeriId",
                table: "SpPermohonanAktiviti",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti_JSukanId",
                table: "SpPermohonanAktiviti",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti_JTahapAktivitiId",
                table: "SpPermohonanAktiviti",
                column: "JTahapAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti1_AkCartaId",
                table: "SpPermohonanAktiviti1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti1_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti1",
                column: "SpPermohonanAktivitiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2",
                column: "SpPermohonanAktivitiId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti1");

            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti2");

            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti");

            migrationBuilder.DropTable(
                name: "JSukan");

            migrationBuilder.DropTable(
                name: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "FlCetak",
                table: "AkPO");
        }
    }
}
