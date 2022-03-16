using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblJPelulus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPOLaras_AkPO_AkPOId",
                table: "AkPOLaras");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja");

            migrationBuilder.DropTable(
                name: "JJawatanPekerja");

            migrationBuilder.DropIndex(
                name: "IX_SuPekerja_JJawatanPekerjaId",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "JJawatanPekerjaId",
                table: "SuPekerja");

            migrationBuilder.AddColumn<string>(
                name: "Jawatan",
                table: "SuPekerja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JPelulus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: false),
                    MinAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaksAmaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsNotaMinta = table.Column<bool>(type: "bit", nullable: false),
                    IsPO = table.Column<bool>(type: "bit", nullable: false),
                    IsBelian = table.Column<bool>(type: "bit", nullable: false),
                    IsPV = table.Column<bool>(type: "bit", nullable: false),
                    IsPendahuluan = table.Column<bool>(type: "bit", nullable: false),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPelulus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPelulus_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JPelulus_SuPekerjaId",
                table: "JPelulus",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPOLaras_AkPO_AkPOId",
                table: "AkPOLaras",
                column: "AkPOId",
                principalTable: "AkPO",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPOLaras_AkPO_AkPOId",
                table: "AkPOLaras");

            migrationBuilder.DropTable(
                name: "JPelulus");

            migrationBuilder.DropColumn(
                name: "Jawatan",
                table: "SuPekerja");

            migrationBuilder.AddColumn<int>(
                name: "JJawatanPekerjaId",
                table: "SuPekerja",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JJawatanPekerja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    Kod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JJawatanPekerja", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JJawatanPekerjaId",
                table: "SuPekerja",
                column: "JJawatanPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPOLaras_AkPO_AkPOId",
                table: "AkPOLaras",
                column: "AkPOId",
                principalTable: "AkPO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja",
                column: "JJawatanPekerjaId",
                principalTable: "JJawatanPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
