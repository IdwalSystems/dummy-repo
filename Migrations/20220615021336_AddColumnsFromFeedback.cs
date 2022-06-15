using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddColumnsFromFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "Pengelolaan",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "Penyertaan",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "Pertandingan",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "ProgramBinaan",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.AddColumn<string>(
                name: "Catatan",
                table: "SuProfil1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AkCartaId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsElit",
                table: "JSukan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPembangunan",
                table: "JSukan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FlJenis",
                table: "AkNotaMinta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "Catatan",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "IsElit",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "IsPembangunan",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "FlJenis",
                table: "AkNotaMinta");

            migrationBuilder.AlterColumn<int>(
                name: "AkCartaId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pengelolaan",
                table: "SpPendahuluanPelbagai",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Penyertaan",
                table: "SpPendahuluanPelbagai",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pertandingan",
                table: "SpPendahuluanPelbagai",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ProgramBinaan",
                table: "SpPendahuluanPelbagai",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
