using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblAkBankRecon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkPadananPenyataId",
                table: "AkTerima2",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkPadananPenyataId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkBankRecon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Bulan = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    BakiPenyata = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlMuatNaik = table.Column<int>(type: "int", nullable: false),
                    TarMuatNaik = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsKunci = table.Column<bool>(type: "bit", nullable: false),
                    TarKunci = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_AkBankRecon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBankRecon_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPadananPenyata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkBankReconPenyataBankId = table.Column<int>(type: "int", nullable: false),
                    FlJenis = table.Column<int>(type: "int", nullable: false),
                    AkPVId = table.Column<int>(type: "int", nullable: true),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPadananPenyata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AkBankReconPenyataBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkBankReconId = table.Column<int>(type: "int", nullable: false),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KodTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerihalTransaksi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoDokumen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Baki = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkBankReconPenyataBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkBankReconPenyataBank_AkBankRecon_AkBankReconId",
                        column: x => x.AkBankReconId,
                        principalTable: "AkBankRecon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_AkPadananPenyataId",
                table: "AkTerima2",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkPadananPenyataId",
                table: "AkPV",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankRecon_AkBankId",
                table: "AkBankRecon",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankReconPenyataBank_AkBankReconId",
                table: "AkBankReconPenyataBank",
                column: "AkBankReconId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankReconPenyataBank_AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                column: "AkPadananPenyataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_AkPadananPenyata_AkPadananPenyataId",
                table: "AkPV",
                column: "AkPadananPenyataId",
                principalTable: "AkPadananPenyata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTerima2_AkPadananPenyata_AkPadananPenyataId",
                table: "AkTerima2",
                column: "AkPadananPenyataId",
                principalTable: "AkPadananPenyata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_AkPadananPenyata_AkPadananPenyataId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima2_AkPadananPenyata_AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropTable(
                name: "AkBankReconPenyataBank");

            migrationBuilder.DropTable(
                name: "AkBankRecon");

            migrationBuilder.DropTable(
                name: "AkPadananPenyata");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima2_AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_AkPadananPenyataId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "AkPadananPenyataId",
                table: "AkPV");
        }
    }
}
