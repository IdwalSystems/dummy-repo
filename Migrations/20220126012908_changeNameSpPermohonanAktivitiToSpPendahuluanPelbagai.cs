using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class changeNameSpPermohonanAktivitiToSpPendahuluanPelbagai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti1");

            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti2");

            migrationBuilder.DropTable(
                name: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "NoPO",
                table: "AkNotaMinta");

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPermohonan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JenisPermohonan = table.Column<int>(type: "int", nullable: false),
                    Penyertaan = table.Column<bool>(type: "bit", nullable: false),
                    Pertandingan = table.Column<bool>(type: "bit", nullable: false),
                    Pengelolaan = table.Column<bool>(type: "bit", nullable: false),
                    ProgramBinaan = table.Column<bool>(type: "bit", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktiviti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tempat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JTahapId = table.Column<int>(type: "int", nullable: false),
                    JumAtl = table.Column<int>(type: "int", nullable: false),
                    JumJul = table.Column<int>(type: "int", nullable: false),
                    JumPeg = table.Column<int>(type: "int", nullable: false),
                    JumTek = table.Column<int>(type: "int", nullable: false),
                    JumUru = table.Column<int>(type: "int", nullable: false),
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
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JTahapAktivitiId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                        column: x => x.JTahapAktivitiId,
                        principalTable: "JTahapAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kadar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bil = table.Column<int>(type: "int", nullable: false),
                    Bln = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai1_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpPendahuluanPelbagai2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BilAtl = table.Column<int>(type: "int", nullable: false),
                    BilJul = table.Column<int>(type: "int", nullable: false),
                    BilPeg = table.Column<int>(type: "int", nullable: false),
                    BilTek = table.Column<int>(type: "int", nullable: false),
                    BilUru = table.Column<int>(type: "int", nullable: false),
                    JumL = table.Column<int>(type: "int", nullable: false),
                    JumP = table.Column<int>(type: "int", nullable: false),
                    SpPendahuluanPelbagaiId = table.Column<int>(type: "int", nullable: false),
                    JJantinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPendahuluanPelbagai2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai2_JJantina_JJantinaId",
                        column: x => x.JJantinaId,
                        principalTable: "JJantina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPendahuluanPelbagai2_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                        column: x => x.SpPendahuluanPelbagaiId,
                        principalTable: "SpPendahuluanPelbagai",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JKWId",
                table: "SpPendahuluanPelbagai",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JNegeriId",
                table: "SpPendahuluanPelbagai",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JSukanId",
                table: "SpPendahuluanPelbagai",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                column: "JTahapAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_AkCartaId",
                table: "SpPendahuluanPelbagai1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_SpPendahuluanPelbagaiId",
                table: "SpPendahuluanPelbagai1",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai2_JJantinaId",
                table: "SpPendahuluanPelbagai2",
                column: "JJantinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai2_SpPendahuluanPelbagaiId",
                table: "SpPendahuluanPelbagai2",
                column: "SpPendahuluanPelbagaiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai1");

            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai2");

            migrationBuilder.DropTable(
                name: "SpPendahuluanPelbagai");

            migrationBuilder.AddColumn<string>(
                name: "NoPO",
                table: "AkNotaMinta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpPermohonanAktiviti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Aktiviti = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    JTahapAktivitiId = table.Column<int>(type: "int", nullable: true),
                    JTahapId = table.Column<int>(type: "int", nullable: false),
                    JenisPermohonan = table.Column<int>(type: "int", nullable: false),
                    JumAtl = table.Column<int>(type: "int", nullable: false),
                    JumJul = table.Column<int>(type: "int", nullable: false),
                    JumKeseluruhan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JumLulus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JumPeg = table.Column<int>(type: "int", nullable: false),
                    JumSokong = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JumTek = table.Column<int>(type: "int", nullable: false),
                    JumUru = table.Column<int>(type: "int", nullable: false),
                    NoPermohonan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pelulus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pengelolaan = table.Column<bool>(type: "bit", nullable: false),
                    Penyedia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Penyertaan = table.Column<bool>(type: "bit", nullable: false),
                    Penyokong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pertandingan = table.Column<bool>(type: "bit", nullable: false),
                    ProgramBinaan = table.Column<bool>(type: "bit", nullable: false),
                    StatusLulus = table.Column<int>(type: "int", nullable: false),
                    StatusSokong = table.Column<int>(type: "int", nullable: false),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarLulus = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarSedia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarSokong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tarikh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tempat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPermohonanAktiviti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Bil = table.Column<int>(type: "int", nullable: false),
                    Bln = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kadar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpPermohonanAktivitiId = table.Column<int>(type: "int", nullable: false)
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
                    BilAtl = table.Column<int>(type: "int", nullable: false),
                    BilJul = table.Column<int>(type: "int", nullable: false),
                    BilPeg = table.Column<int>(type: "int", nullable: false),
                    BilTek = table.Column<int>(type: "int", nullable: false),
                    BilUru = table.Column<int>(type: "int", nullable: false),
                    JJantinaId = table.Column<int>(type: "int", nullable: false),
                    JumL = table.Column<int>(type: "int", nullable: false),
                    JumP = table.Column<int>(type: "int", nullable: false),
                    SpPermohonanAktivitiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpPermohonanAktiviti2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti2_JJantina_JJantinaId",
                        column: x => x.JJantinaId,
                        principalTable: "JJantina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpPermohonanAktiviti2_SpPermohonanAktiviti_SpPermohonanAktivitiId",
                        column: x => x.SpPermohonanAktivitiId,
                        principalTable: "SpPermohonanAktiviti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti_JKWId",
                table: "SpPermohonanAktiviti",
                column: "JKWId");

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
                column: "SpPermohonanAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_JJantinaId",
                table: "SpPermohonanAktiviti2",
                column: "JJantinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2",
                column: "SpPermohonanAktivitiId");
        }
    }
}
