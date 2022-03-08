using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class ubahTableSpPendahuluanAddSuPerkerjaId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Penyedia",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_SuPekerjaId",
                table: "SpPendahuluanPelbagai",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_SuPekerja_SuPekerjaId",
                table: "SpPendahuluanPelbagai",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_SuPekerja_SuPekerjaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai_SuPekerjaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "SuPekerjaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.AddColumn<string>(
                name: "Penyedia",
                table: "SpPendahuluanPelbagai",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
