using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTableSuAtletnSuJurulatih : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "SuAtlet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodAtlet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    TarikhAktif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBerhenti = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: true),
                    JAgamaId = table.Column<int>(type: "int", nullable: true),
                    JBangsaId = table.Column<int>(type: "int", nullable: true),
                    Jawatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuAtlet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JAgama_JAgamaId",
                        column: x => x.JAgamaId,
                        principalTable: "JAgama",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JBangsa_JBangsaId",
                        column: x => x.JBangsaId,
                        principalTable: "JBangsa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuAtlet_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuJurulatih",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodJurulatih = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JNegeriId = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Emel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FlStatus = table.Column<int>(type: "int", nullable: false),
                    TarikhAktif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBerhenti = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: true),
                    JAgamaId = table.Column<int>(type: "int", nullable: true),
                    JBangsaId = table.Column<int>(type: "int", nullable: true),
                    Jawatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: true),
                    NoAkaunBank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JSukanId = table.Column<int>(type: "int", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuJurulatih", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JAgama_JAgamaId",
                        column: x => x.JAgamaId,
                        principalTable: "JAgama",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JBangsa_JBangsaId",
                        column: x => x.JBangsaId,
                        principalTable: "JBangsa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JNegeri_JNegeriId",
                        column: x => x.JNegeriId,
                        principalTable: "JNegeri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuJurulatih_JSukan_JSukanId",
                        column: x => x.JSukanId,
                        principalTable: "JSukan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JAgamaId",
                table: "SuAtlet",
                column: "JAgamaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JBangsaId",
                table: "SuAtlet",
                column: "JBangsaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JBankId",
                table: "SuAtlet",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JCaraBayarId",
                table: "SuAtlet",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JNegeriId",
                table: "SuAtlet",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SuAtlet_JSukanId",
                table: "SuAtlet",
                column: "JSukanId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JAgamaId",
                table: "SuJurulatih",
                column: "JAgamaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JBangsaId",
                table: "SuJurulatih",
                column: "JBangsaId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JBankId",
                table: "SuJurulatih",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JCaraBayarId",
                table: "SuJurulatih",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JNegeriId",
                table: "SuJurulatih",
                column: "JNegeriId");

            migrationBuilder.CreateIndex(
                name: "IX_SuJurulatih_JSukanId",
                table: "SuJurulatih",
                column: "JSukanId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "SuAtlet");

            migrationBuilder.DropTable(
                name: "SuJurulatih");

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
        }
    }
}
