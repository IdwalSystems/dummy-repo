using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldAkJurnalIdTblAkPadananPenyata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_AkPadananPenyata_AkPadananPenyataId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima2_AkPadananPenyata_AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima2_AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_AkPadananPenyataId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkBankReconPenyataBank_AkPadananPenyataId",
                table: "AkBankReconPenyataBank");

            migrationBuilder.DropColumn(
                name: "AkPadananPenyataId",
                table: "AkTerima2");

            migrationBuilder.DropColumn(
                name: "AkPadananPenyataId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "AkPadananPenyataId",
                table: "AkBankReconPenyataBank");

            migrationBuilder.AddColumn<int>(
                name: "AkJurnalId",
                table: "AkPadananPenyata",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPadan",
                table: "AkBankReconPenyataBank",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AkJurnalAkPadananPenyata",
                columns: table => new
                {
                    AkJurnalId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkJurnalAkPadananPenyata", x => new { x.AkJurnalId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkJurnalAkPadananPenyata_AkJurnal_AkJurnalId",
                        column: x => x.AkJurnalId,
                        principalTable: "AkJurnal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkJurnalAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPadananPenyataAkTerima2",
                columns: table => new
                {
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false),
                    AkTerima2Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPadananPenyataAkTerima2", x => new { x.AkPadananPenyataId, x.AkTerima2Id });
                    table.ForeignKey(
                        name: "FK_AkPadananPenyataAkTerima2_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPadananPenyataAkTerima2_AkTerima2_AkTerima2Id",
                        column: x => x.AkTerima2Id,
                        principalTable: "AkTerima2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkPVAkPadananPenyata",
                columns: table => new
                {
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVAkPadananPenyata", x => new { x.AkPVId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkPVAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVAkPadananPenyata_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPadananPenyata_AkBankReconPenyataBankId",
                table: "AkPadananPenyata",
                column: "AkBankReconPenyataBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnalAkPadananPenyata_AkPadananPenyataId",
                table: "AkJurnalAkPadananPenyata",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPadananPenyataAkTerima2_AkTerima2Id",
                table: "AkPadananPenyataAkTerima2",
                column: "AkTerima2Id");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVAkPadananPenyata_AkPadananPenyataId",
                table: "AkPVAkPadananPenyata",
                column: "AkPadananPenyataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPadananPenyata_AkBankReconPenyataBank_AkBankReconPenyataBankId",
                table: "AkPadananPenyata",
                column: "AkBankReconPenyataBankId",
                principalTable: "AkBankReconPenyataBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPadananPenyata_AkBankReconPenyataBank_AkBankReconPenyataBankId",
                table: "AkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkJurnalAkPadananPenyata");

            migrationBuilder.DropTable(
                name: "AkPadananPenyataAkTerima2");

            migrationBuilder.DropTable(
                name: "AkPVAkPadananPenyata");

            migrationBuilder.DropIndex(
                name: "IX_AkPadananPenyata_AkBankReconPenyataBankId",
                table: "AkPadananPenyata");

            migrationBuilder.DropColumn(
                name: "AkJurnalId",
                table: "AkPadananPenyata");

            migrationBuilder.DropColumn(
                name: "IsPadan",
                table: "AkBankReconPenyataBank");

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

            migrationBuilder.AddColumn<int>(
                name: "AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima2_AkPadananPenyataId",
                table: "AkTerima2",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkPadananPenyataId",
                table: "AkPV",
                column: "AkPadananPenyataId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBankReconPenyataBank_AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                column: "AkPadananPenyataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                column: "AkPadananPenyataId",
                principalTable: "AkPadananPenyata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
