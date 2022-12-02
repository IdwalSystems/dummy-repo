using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTableAkNotaDebitKreditBelian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkBelianId = table.Column<int>(type: "int", nullable: false),
                    FlJenis = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian_AkBelian_AkBelianId",
                        column: x => x.AkBelianId,
                        principalTable: "AkBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaDebitKreditBelianId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian1_AkNotaDebitKreditBelian_AkNotaDebitKreditBelianId",
                        column: x => x.AkNotaDebitKreditBelianId,
                        principalTable: "AkNotaDebitKreditBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaDebitKreditBelian2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaDebitKreditBelianId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaDebitKreditBelian2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaDebitKreditBelian2_AkNotaDebitKreditBelian_AkNotaDebitKreditBelianId",
                        column: x => x.AkNotaDebitKreditBelianId,
                        principalTable: "AkNotaDebitKreditBelian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian_AkBelianId",
                table: "AkNotaDebitKreditBelian",
                column: "AkBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian_JBahagianId",
                table: "AkNotaDebitKreditBelian",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian1_AkCartaId",
                table: "AkNotaDebitKreditBelian1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian1_AkNotaDebitKreditBelianId",
                table: "AkNotaDebitKreditBelian1",
                column: "AkNotaDebitKreditBelianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaDebitKreditBelian2_AkNotaDebitKreditBelianId",
                table: "AkNotaDebitKreditBelian2",
                column: "AkNotaDebitKreditBelianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian1");

            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian2");

            migrationBuilder.DropTable(
                name: "AkNotaDebitKreditBelian");
        }
    }
}
