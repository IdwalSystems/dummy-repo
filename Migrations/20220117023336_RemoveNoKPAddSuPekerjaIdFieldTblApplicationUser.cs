using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveNoKPAddSuPekerjaIdFieldTblApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoKP",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SuPekerjaId",
                table: "AspNetUsers",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SuPekerja_SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuPekerjaId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "NoKP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
