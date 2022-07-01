using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblPenghutangTblInvoisTblAkTerima3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkPenghutangId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkPenghutang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSykt = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    NamaSykt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Alamat1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alamat2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Bandar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkaunBank = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPenghutang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPenghutang_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPenghutang_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInvois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoInbois = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkPOId = table.Column<int>(type: "int", nullable: true),
                    KodObjekAPId = table.Column<int>(type: "int", nullable: false),
                    AkPenghutangId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInvois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois_AkCarta_KodObjekAPId",
                        column: x => x.KodObjekAPId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_AkPenghutang_AkPenghutangId",
                        column: x => x.AkPenghutangId,
                        principalTable: "AkPenghutang",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_AkPO_AkPOId",
                        column: x => x.AkPOId,
                        principalTable: "AkPO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInvois_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkInvois1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkInvoisId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInvois1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkInvois1_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInvois2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkInvoisId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AkInvois2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInvois2_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkTerima3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTerimaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AkInvoisId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTerima3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTerima3_AkInvois_AkInvoisId",
                        column: x => x.AkInvoisId,
                        principalTable: "AkInvois",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTerima3_AkTerima_AkTerimaId",
                        column: x => x.AkTerimaId,
                        principalTable: "AkTerima",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkPenghutangId",
                table: "AkTerima",
                column: "AkPenghutangId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_AkPenghutangId",
                table: "AkInvois",
                column: "AkPenghutangId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_AkPOId",
                table: "AkInvois",
                column: "AkPOId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JBahagianId",
                table: "AkInvois",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JKWId",
                table: "AkInvois",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_KodObjekAPId",
                table: "AkInvois",
                column: "KodObjekAPId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois1_AkCartaId",
                table: "AkInvois1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois1_AkInvoisId",
                table: "AkInvois1",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois2_AkInvoisId",
                table: "AkInvois2",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenghutang_JBankId",
                table: "AkPenghutang",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPenghutang_JNegeriId",
                table: "AkPenghutang",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima3_AkInvoisId",
                table: "AkTerima3",
                column: "AkInvoisId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima3_AkTerimaId",
                table: "AkTerima3",
                column: "AkTerimaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTerima_AkPenghutang_AkPenghutangId",
                table: "AkTerima",
                column: "AkPenghutangId",
                principalTable: "AkPenghutang",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima_AkPenghutang_AkPenghutangId",
                table: "AkTerima");

            migrationBuilder.DropTable(
                name: "AkInvois1");

            migrationBuilder.DropTable(
                name: "AkInvois2");

            migrationBuilder.DropTable(
                name: "AkTerima3");

            migrationBuilder.DropTable(
                name: "AkInvois");

            migrationBuilder.DropTable(
                name: "AkPenghutang");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima_AkPenghutangId",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "AkPenghutangId",
                table: "AkTerima");
        }
    }
}
