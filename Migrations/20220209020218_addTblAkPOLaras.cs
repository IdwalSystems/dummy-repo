using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblAkPOLaras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AkPOLaras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPOLaras = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    AkPOId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPOLaras_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkPOLaras1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOLarasId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPOLaras1_AkPOLaras_AkPOLarasId",
                        column: x => x.AkPOLarasId,
                        principalTable: "AkPOLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPOLaras2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPOLarasId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Baris = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPOLaras2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPOLaras2_AkPOLaras_AkPOLarasId",
                        column: x => x.AkPOLarasId,
                        principalTable: "AkPOLaras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_AkPOId",
                table: "AkPOLaras",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_JKWId",
                table: "AkPOLaras",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras1_AkCartaId",
                table: "AkPOLaras1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras1_AkPOLarasId",
                table: "AkPOLaras1",
                column: "AkPOLarasId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras2_AkPOLarasId",
                table: "AkPOLaras2",
                column: "AkPOLarasId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkPOLaras1");

            migrationBuilder.DropTable(
                name: "AkPOLaras2");

            migrationBuilder.DropTable(
                name: "AkPOLaras");
        }
    }
}
