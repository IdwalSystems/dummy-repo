using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblAkPenyataPemungut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "JenisCek",
                table: "AkTerima2",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkPenyataPemungutId",
                table: "AkTerima2",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoDokumen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoSlip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarSlip = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlJenisCek = table.Column<int>(type: "int", nullable: false),
                    BilTerima = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkPenyataPemungutId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_AkPenyataPemungut_AkPenyataPemungutId",
                        column: x => x.AkPenyataPemungutId,
                        principalTable: "AkPenyataPemungut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut1_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPenyataPemungut2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    AkPenyataPemungutId = table.Column<int>(type: "int", nullable: false),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenyataPemungut2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut2_AkPenyataPemungut_AkPenyataPemungutId",
                        column: x => x.AkPenyataPemungutId,
                        principalTable: "AkPenyataPemungut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPenyataPemungut2_AkTerima2_AkTerima2Id",
                        column: x => x.AkTerima2Id,
                        principalTable: "AkTerima2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_AkBankId",
                table: "AkPenyataPemungut",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_JCaraBayarId",
                table: "AkPenyataPemungut",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut_SuPekerjaId",
                table: "AkPenyataPemungut",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_AkCartaId",
                table: "AkPenyataPemungut1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_AkPenyataPemungutId",
                table: "AkPenyataPemungut1",
                column: "AkPenyataPemungutId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut1_JBahagianId",
                table: "AkPenyataPemungut1",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut2_AkPenyataPemungutId",
                table: "AkPenyataPemungut2",
                column: "AkPenyataPemungutId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenyataPemungut2_AkTerima2Id",
                table: "AkPenyataPemungut2",
                column: "AkTerima2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkPenyataPemungut1");

            migrationBuilder.DropTable(
                name: "AkPenyataPemungut2");

            migrationBuilder.DropTable(
                name: "AkPenyataPemungut");

            migrationBuilder.DropColumn(
                name: "AkPenyataPemungutId",
                table: "AkTerima2");

            migrationBuilder.AlterColumn<string>(
                name: "JenisCek",
                table: "AkTerima2",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
