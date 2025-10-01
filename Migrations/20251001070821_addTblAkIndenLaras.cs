using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblAkIndenLaras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AkIndenLaras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoRujukan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    TarBatal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SebabHapus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    AkIndenId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaMasukId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaKemaskiniId = table.Column<int>(type: "int", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkIndenLaras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkIndenLaras_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkIndenLaras_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkIndenLaras_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkIndenLaras1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenLarasId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkIndenLaras1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkIndenLaras1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkIndenLaras1_AkIndenLaras_AkIndenLarasId",
                        column: x => x.AkIndenLarasId,
                        principalTable: "AkIndenLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkIndenLaras2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenLarasId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkIndenLaras2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkIndenLaras2_AkIndenLaras_AkIndenLarasId",
                        column: x => x.AkIndenLarasId,
                        principalTable: "AkIndenLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras_AkIndenId",
                table: "AkIndenLaras",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras_JBahagianId",
                table: "AkIndenLaras",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras_JKWId",
                table: "AkIndenLaras",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras1_AkCartaId",
                table: "AkIndenLaras1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras1_AkIndenLarasId",
                table: "AkIndenLaras1",
                column: "AkIndenLarasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkIndenLaras2_AkIndenLarasId",
                table: "AkIndenLaras2",
                column: "AkIndenLarasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkIndenLaras1");

            migrationBuilder.DropTable(
                name: "AkIndenLaras2");

            migrationBuilder.DropTable(
                name: "AkIndenLaras");
        }
    }
}
