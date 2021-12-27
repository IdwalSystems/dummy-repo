using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddInitialTables4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkCarta_JKW_JKWId",
                table: "AkCarta");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCarta_JKW_JKWId",
                table: "AkCarta",
                column: "JKWId",
                principalTable: "JKW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkCarta_JKW_JKWId",
                table: "AkCarta");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCarta_JKW_JKWId",
                table: "AkCarta",
                column: "JKWId",
                principalTable: "JKW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
