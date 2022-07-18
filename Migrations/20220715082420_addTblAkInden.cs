using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblAkInden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlPO",
                table: "AkBelian",
                newName: "FlTanggungan");

            migrationBuilder.AddColumn<int>(
                name: "AkIndenId",
                table: "AkBelian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlJenisTanggungan",
                table: "AkBelian",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AkInden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoInden = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tarikh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhBekalan = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TarikhPosting = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Jumlah = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tahun = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    TempohSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhSiap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tajuk = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FlPosting = table.Column<int>(type: "int", nullable: false),
                    FlCetak = table.Column<int>(type: "int", nullable: false),
                    IsInKewangan = table.Column<bool>(type: "bit", nullable: false),
                    AkPembekalId = table.Column<int>(type: "int", nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: false),
                    JBahagianId = table.Column<int>(type: "int", nullable: true),
                    AkNotaMintaId = table.Column<int>(type: "int", nullable: true),
                    AkCartaId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden_AkNotaMinta_AkNotaMintaId",
                        column: x => x.AkNotaMintaId,
                        principalTable: "AkNotaMinta",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkInden_AkPembekal_AkPembekalId",
                        column: x => x.AkPembekalId,
                        principalTable: "AkPembekal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden_JBahagian_JBahagianId",
                        column: x => x.JBahagianId,
                        principalTable: "JBahagian",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkInden_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AkInden1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenId = table.Column<int>(type: "int", nullable: false),
                    AkCartaId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden1_AkCarta_AkCartaId",
                        column: x => x.AkCartaId,
                        principalTable: "AkCarta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkInden1_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AkInden2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkIndenId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    Bil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoStok = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Kuantiti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Harga = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkInden2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkInden2_AkInden_AkIndenId",
                        column: x => x.AkIndenId,
                        principalTable: "AkInden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_AkIndenId",
                table: "AkBelian",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_AkCartaId",
                table: "AkInden",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_AkNotaMintaId",
                table: "AkInden",
                column: "AkNotaMintaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_AkPembekalId",
                table: "AkInden",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_JBahagianId",
                table: "AkInden",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden_JKWId",
                table: "AkInden",
                column: "JKWId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden1_AkCartaId",
                table: "AkInden1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden1_AkIndenId",
                table: "AkInden1",
                column: "AkIndenId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInden2_AkIndenId",
                table: "AkInden2",
                column: "AkIndenId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_AkInden_AkIndenId",
                table: "AkBelian",
                column: "AkIndenId",
                principalTable: "AkInden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_AkInden_AkIndenId",
                table: "AkBelian");

            migrationBuilder.DropTable(
                name: "AkInden1");

            migrationBuilder.DropTable(
                name: "AkInden2");

            migrationBuilder.DropTable(
                name: "AkInden");

            migrationBuilder.DropIndex(
                name: "IX_AkBelian_AkIndenId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "AkIndenId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "FlJenisTanggungan",
                table: "AkBelian");

            migrationBuilder.RenameColumn(
                name: "FlTanggungan",
                table: "AkBelian",
                newName: "FlPO");
        }
    }
}
