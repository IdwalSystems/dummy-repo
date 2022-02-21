using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldSpPendhuluanPelbagaiIdIntoTblAkTerimaAkPV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpPendahuluanPelbagaiId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpPendahuluanPelbagaiId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_SpPendahuluanPelbagaiId",
                table: "AkTerima",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SpPendahuluanPelbagaiId",
                table: "AkPV",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkPV",
                column: "SpPendahuluanPelbagaiId",
                principalTable: "SpPendahuluanPelbagai",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTerima_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkTerima",
                column: "SpPendahuluanPelbagaiId",
                principalTable: "SpPendahuluanPelbagai",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkTerima");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima_SpPendahuluanPelbagaiId",
                table: "AkTerima");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_SpPendahuluanPelbagaiId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SpPendahuluanPelbagaiId",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "SpPendahuluanPelbagaiId",
                table: "AkPV");
        }
    }
}
