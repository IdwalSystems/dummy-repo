using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblJPenyemak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JPenyemak",
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
                    table.PrimaryKey("PK_JPenyemak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPenyemak_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JPenyemak_SuPekerjaId",
                table: "JPenyemak",
                column: "SuPekerjaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JPenyemak");
        }
    }
}
