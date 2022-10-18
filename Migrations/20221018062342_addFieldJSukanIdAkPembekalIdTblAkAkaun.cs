using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJSukanIdAkPembekalIdTblAkAkaun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkPembekalId",
                table: "AkAkaun",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkPenghutangId",
                table: "AkAkaun",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JSukanId",
                table: "AkAkaun",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppLog_SuPekerjaId",
                table: "AppLog",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppLog_SuPekerja_SuPekerjaId",
                table: "AppLog",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppLog_SuPekerja_SuPekerjaId",
                table: "AppLog");

            migrationBuilder.DropIndex(
                name: "IX_AppLog_SuPekerjaId",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "AkPembekalId",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "AkPenghutangId",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "JSukanId",
                table: "AkAkaun");
        }
    }
}
