using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class UpdateFieldVotIdTblAbBukuVot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbBukuVot_AkCarta_AkCartaId",
                table: "AbBukuVot");

            migrationBuilder.DropIndex(
                name: "IX_AbBukuVot_AkCartaId",
                table: "AbBukuVot");

            migrationBuilder.DropColumn(
                name: "AkCartaId",
                table: "AbBukuVot");

            migrationBuilder.AlterColumn<int>(
                name: "VotId",
                table: "AbBukuVot",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_VotId",
                table: "AbBukuVot",
                column: "VotId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbBukuVot_AkCarta_VotId",
                table: "AbBukuVot",
                column: "VotId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbBukuVot_AkCarta_VotId",
                table: "AbBukuVot");

            migrationBuilder.DropIndex(
                name: "IX_AbBukuVot_VotId",
                table: "AbBukuVot");

            migrationBuilder.AlterColumn<string>(
                name: "VotId",
                table: "AbBukuVot",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AkCartaId",
                table: "AbBukuVot",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_AkCartaId",
                table: "AbBukuVot",
                column: "AkCartaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbBukuVot_AkCarta_AkCartaId",
                table: "AbBukuVot",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
