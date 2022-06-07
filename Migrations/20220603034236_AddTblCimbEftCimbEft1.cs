using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblCimbEftCimbEft1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_SuAtlet_SuAtletId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_SuJurulatih_SuJurulatihId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_SuAtlet_SuAtletId",
                table: "AkTunaiCV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_SuJurulatih_SuJurulatihId",
                table: "AkTunaiCV");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_SuAtletId",
                table: "AkTunaiCV");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_SuJurulatihId",
                table: "AkTunaiCV");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_SuAtletId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_SuJurulatihId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SuAtletId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "SuJurulatihId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "SuAtletId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SuJurulatihId",
                table: "AkPV");

            migrationBuilder.AddColumn<int>(
                name: "JCaraBayarId",
                table: "SuProfil1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoCekEFT",
                table: "SuProfil1",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarCekEFT",
                table: "SuProfil1",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkCimbEFT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoPBI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarJana = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarBayar = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NamaFail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BilPV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlKategori = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCimbEFT", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkCimbEFT1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkCimbEFTId = table.Column<int>(type: "int", nullable: false),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    FlPenerimaEFT = table.Column<int>(type: "int", nullable: false),
                    PenerimaId = table.Column<int>(type: "int", nullable: true),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    FlStatus = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkCimbEFT1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkCimbEFT_AkCimbEFTId",
                        column: x => x.AkCimbEFTId,
                        principalTable: "AkCimbEFT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkPembekal_PenerimaId",
                        column: x => x.PenerimaId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuAtlet_PenerimaId",
                        column: x => x.PenerimaId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuJurulatih_PenerimaId",
                        column: x => x.PenerimaId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkCimbEFT1_SuPekerja_PenerimaId",
                        column: x => x.PenerimaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JCaraBayarId",
                table: "SuProfil1",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT_AkBankId",
                table: "AkCimbEFT",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT_SuPekerjaId",
                table: "AkCimbEFT",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkBankId",
                table: "AkCimbEFT1",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkCimbEFTId",
                table: "AkCimbEFT1",
                column: "AkCimbEFTId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkPVId",
                table: "AkCimbEFT1",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_PenerimaId",
                table: "AkCimbEFT1",
                column: "PenerimaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuProfil1_JCaraBayar_JCaraBayarId",
                table: "SuProfil1",
                column: "JCaraBayarId",
                principalTable: "JCaraBayar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuProfil1_JCaraBayar_JCaraBayarId",
                table: "SuProfil1");

            migrationBuilder.DropTable(
                name: "AkCimbEFT1");

            migrationBuilder.DropTable(
                name: "AkCimbEFT");

            migrationBuilder.DropIndex(
                name: "IX_SuProfil1_JCaraBayarId",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "JCaraBayarId",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "NoCekEFT",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "TarCekEFT",
                table: "SuProfil1");

            migrationBuilder.AddColumn<int>(
                name: "SuAtletId",
                table: "AkTunaiCV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuJurulatihId",
                table: "AkTunaiCV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuAtletId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuJurulatihId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_SuAtletId",
                table: "AkTunaiCV",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_SuJurulatihId",
                table: "AkTunaiCV",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuAtletId",
                table: "AkPV",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuJurulatihId",
                table: "AkPV",
                column: "SuJurulatihId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_SuAtlet_SuAtletId",
                table: "AkPV",
                column: "SuAtletId",
                principalTable: "SuAtlet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_SuJurulatih_SuJurulatihId",
                table: "AkPV",
                column: "SuJurulatihId",
                principalTable: "SuJurulatih",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_SuAtlet_SuAtletId",
                table: "AkTunaiCV",
                column: "SuAtletId",
                principalTable: "SuAtlet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_SuJurulatih_SuJurulatihId",
                table: "AkTunaiCV",
                column: "SuJurulatihId",
                principalTable: "SuJurulatih",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
