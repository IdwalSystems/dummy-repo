using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblAkTunaiPanjarAkTunaiPemegang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AkTunaiPanjar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KaunterPanjar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiPanjar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiPanjar_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiPanjar_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiPemegang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false),
                    AkTunaiPanjarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiPemegang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_AkTunaiPanjar_AkTunaiPanjarId",
                        column: x => x.AkTunaiPanjarId,
                        principalTable: "AkTunaiPanjar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPanjar_AkCartaId",
                table: "AkTunaiPanjar",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPanjar_JKWId",
                table: "AkTunaiPanjar",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_AkTunaiPanjarId",
                table: "AkTunaiPemegang",
                column: "AkTunaiPanjarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_SuPekerjaId",
                table: "AkTunaiPemegang",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AkTunaiPemegang");

            migrationBuilder.DropTable(
                name: "AkTunaiPanjar");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
