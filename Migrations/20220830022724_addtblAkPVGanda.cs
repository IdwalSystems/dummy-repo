using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addtblAkPVGanda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGanda",
                table: "AkPV",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AkPVGanda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AkPVId = table.Column<int>(type: "int", nullable: false),
                    Indek = table.Column<int>(type: "int", nullable: false),
                    FlKategoriPenerima = table.Column<int>(type: "int", nullable: false),
                    SuPekerjaId = table.Column<int>(type: "int", nullable: true),
                    SuAtletId = table.Column<int>(type: "int", nullable: true),
                    SuJurulatihId = table.Column<int>(type: "int", nullable: true),
                    Nama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoKp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoAkaun = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JBankId = table.Column<int>(type: "int", nullable: false),
                    Amaun = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NoCekAtauEFT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarCekAtauEFT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JCaraBayarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVGanda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_AkPV_AkPVId",
                        column: x => x.AkPVId,
                        principalTable: "AkPV",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_JBank_JBankId",
                        column: x => x.JBankId,
                        principalTable: "JBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_JCaraBayar_JCaraBayarId",
                        column: x => x.JCaraBayarId,
                        principalTable: "JCaraBayar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuAtlet_SuAtletId",
                        column: x => x.SuAtletId,
                        principalTable: "SuAtlet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuJurulatih_SuJurulatihId",
                        column: x => x.SuJurulatihId,
                        principalTable: "SuJurulatih",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AkPVGanda_SuPekerja_SuPekerjaId",
                        column: x => x.SuPekerjaId,
                        principalTable: "SuPekerja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_AkPVId",
                table: "AkPVGanda",
                column: "AkPVId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_JBankId",
                table: "AkPVGanda",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_JCaraBayarId",
                table: "AkPVGanda",
                column: "JCaraBayarId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuAtletId",
                table: "AkPVGanda",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuJurulatihId",
                table: "AkPVGanda",
                column: "SuJurulatihId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGanda_SuPekerjaId",
                table: "AkPVGanda",
                column: "SuPekerjaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkPVGanda");

            migrationBuilder.DropColumn(
                name: "IsGanda",
                table: "AkPV");
        }
    }
}
