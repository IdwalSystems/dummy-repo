using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldAkTunaiRuncitTblAkTunaiLejar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiLejar_AkTunaiCV_AkTunaiCVId",
                table: "AkTunaiLejar");

            migrationBuilder.RenameColumn(
                name: "AkTunaiCVId",
                table: "AkTunaiLejar",
                newName: "AkTunaiRuncitId");

            migrationBuilder.RenameIndex(
                name: "IX_AkTunaiLejar_AkTunaiCVId",
                table: "AkTunaiLejar",
                newName: "IX_AkTunaiLejar_AkTunaiRuncitId");

            migrationBuilder.AddColumn<string>(
                name: "NoRujukan",
                table: "AkTunaiLejar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiLejar_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkTunaiLejar",
                column: "AkTunaiRuncitId",
                principalTable: "AkTunaiRuncit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiLejar_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkTunaiLejar");

            migrationBuilder.DropColumn(
                name: "NoRujukan",
                table: "AkTunaiLejar");

            migrationBuilder.RenameColumn(
                name: "AkTunaiRuncitId",
                table: "AkTunaiLejar",
                newName: "AkTunaiCVId");

            migrationBuilder.RenameIndex(
                name: "IX_AkTunaiLejar_AkTunaiRuncitId",
                table: "AkTunaiLejar",
                newName: "IX_AkTunaiLejar_AkTunaiCVId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiLejar_AkTunaiCV_AkTunaiCVId",
                table: "AkTunaiLejar",
                column: "AkTunaiCVId",
                principalTable: "AkTunaiCV",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
