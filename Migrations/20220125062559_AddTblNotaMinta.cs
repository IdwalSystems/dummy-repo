using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblNotaMinta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuPekerja",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkNotaMintaId",
                table: "AkPO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tajuk",
                table: "AkPO",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkNotaMinta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoRujukan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlBatal = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    NoSiri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoCAS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarikhSeksyenKewangan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaMinta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaMinta1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkNotaMinta1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta1_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkNotaMinta2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AkNotaMinta2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkNotaMinta2_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_AkNotaMintaId",
                table: "AkPO",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_AkPembekalId",
                table: "AkNotaMinta",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JKWId",
                table: "AkNotaMinta",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta1_AkCartaId",
                table: "AkNotaMinta1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta1_AkNotaMintaId",
                table: "AkNotaMinta1",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta2_AkNotaMintaId",
                table: "AkNotaMinta2",
                column: "AkNotaMintaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPO_AkNotaMinta_AkNotaMintaId",
                table: "AkPO",
                column: "AkNotaMintaId",
                principalTable: "AkNotaMinta",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPO_AkNotaMinta_AkNotaMintaId",
                table: "AkPO");

            migrationBuilder.DropTable(
                name: "AkNotaMinta1");

            migrationBuilder.DropTable(
                name: "AkNotaMinta2");

            migrationBuilder.DropTable(
                name: "AkNotaMinta");

            migrationBuilder.DropIndex(
                name: "IX_AkPO_AkNotaMintaId",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "AkNotaMintaId",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "Tajuk",
                table: "AkPO");

            migrationBuilder.AlterColumn<string>(
                name: "Emel",
                table: "SuPekerja",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
