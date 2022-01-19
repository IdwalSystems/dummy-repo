using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addAllTblsForModuleTunaiRuncit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AkTunaiCV",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    NoCV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    AkPembekalId = table.Column<int>(type: "int", nullable: true),
                    Penerima = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alamat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Almat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Catatan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AkBankId = table.Column<int>(type: "int", nullable: false),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiCV", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_AkBank_AkBankId",
                        column: x => x.AkBankId,
                        principalTable: "AkBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiRuncit",
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
                    table.PrimaryKey("PK_AkTunaiRuncit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiRuncit_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkTunaiRuncit_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiCV1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTunaiCVId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiCV1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiCV1_AkTunaiCV_AkTunaiCVId",
                        column: x => x.AkTunaiCVId,
                        principalTable: "AkTunaiCV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkTunaiPemegang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkTunaiRuncitId = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkTunaiPemegang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_AkTunaiRuncit_AkTunaiRuncitId",
                        column: x => x.AkTunaiRuncitId,
                        principalTable: "AkTunaiRuncit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkTunaiPemegang_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkBankId",
                table: "AkTunaiCV",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkPembekalId",
                table: "AkTunaiCV",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_JKWId",
                table: "AkTunaiCV",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_SuPekerjaId",
                table: "AkTunaiCV",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV1_AkCartaId",
                table: "AkTunaiCV1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV1_AkTunaiCVId",
                table: "AkTunaiCV1",
                column: "AkTunaiCVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_AkTunaiRuncitId",
                table: "AkTunaiPemegang",
                column: "AkTunaiRuncitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiPemegang_SuPekerjaId",
                table: "AkTunaiPemegang",
                column: "SuPekerjaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_AkCartaId",
                table: "AkTunaiRuncit",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_JKWId",
                table: "AkTunaiRuncit",
                column: "JKWId");

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
                name: "AkTunaiCV1");

            migrationBuilder.DropTable(
                name: "AkTunaiPemegang");

            migrationBuilder.DropTable(
                name: "AkTunaiCV");

            migrationBuilder.DropTable(
                name: "AkTunaiRuncit");

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
