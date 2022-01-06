using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class tambah_abBukuVot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbBukuVot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Penerima = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Rujukan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tanggungan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tbs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rizab = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Liabiliti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbBukuVot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbBukuVot_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbBukuVot_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_AkCartaId",
                table: "AbBukuVot",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_JKWId",
                table: "AbBukuVot",
                column: "JKWId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbBukuVot");
        }
    }
}
