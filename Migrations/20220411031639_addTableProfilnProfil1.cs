using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTableProfilnProfil1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SuProfil",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bulan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlKategori = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuProfil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuProfil_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SuProfil1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuProfilId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmaunSebelum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tunggakan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuAtletId = table.Column<int>(type: "int", nullable: true),
                    SuJurulatihId = table.Column<int>(type: "int", nullable: true),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuProfil1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuProfil1_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuProfil1_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuAtlet_SuAtletId",
                        column: x => x.SuAtletId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuJurulatih_SuJurulatihId",
                        column: x => x.SuJurulatihId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuProfil1_SuProfil_SuProfilId",
                        column: x => x.SuProfilId,
                        principalTable: "SuProfil",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_AkCartaId",
                table: "SuProfil",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_JBahagianId",
                table: "SuProfil",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil_JKWId",
                table: "SuProfil",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JBankId",
                table: "SuProfil1",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JSukanId",
                table: "SuProfil1",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuAtletId",
                table: "SuProfil1",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuJurulatihId",
                table: "SuProfil1",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_SuProfilId",
                table: "SuProfil1",
                column: "SuProfilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuProfil1");

            migrationBuilder.DropTable(
                name: "SuProfil");
        }
    }
}
