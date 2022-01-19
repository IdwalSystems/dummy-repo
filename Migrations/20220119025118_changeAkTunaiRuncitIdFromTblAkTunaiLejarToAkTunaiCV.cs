using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class changeAkTunaiRuncitIdFromTblAkTunaiLejarToAkTunaiCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkTunaiRuncitId",
                table: "AkTunaiCV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AkTunaiLejar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    AkTunaiCVId = table.Column<int>(type: "int", nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rekup = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiLejar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_AkTunaiCV_AkTunaiCVId",
                        column: x => x.AkTunaiCVId,
                        principalTable: "AkTunaiCV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiLejar_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkTunaiRuncitId",
                table: "AkTunaiCV",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_AkCartaId",
                table: "AkTunaiLejar",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_AkTunaiCVId",
                table: "AkTunaiLejar",
                column: "AkTunaiCVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_JKWId",
                table: "AkTunaiLejar",
                column: "JKWId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkTunaiCV",
                column: "AkTunaiRuncitId",
                principalTable: "AkTunaiRuncit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkTunaiCV");

            migrationBuilder.DropTable(
                name: "AkTunaiLejar");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_AkTunaiRuncitId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "AkTunaiRuncitId",
                table: "AkTunaiCV");
        }
    }
}
