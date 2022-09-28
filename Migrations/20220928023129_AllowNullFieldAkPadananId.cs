using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AllowNullFieldAkPadananId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank");

            migrationBuilder.AlterColumn<int>(
                name: "AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                column: "AkPadananPenyataId",
                principalTable: "AkPadananPenyata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank");

            migrationBuilder.AlterColumn<int>(
                name: "AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AkBankReconPenyataBank_AkPadananPenyata_AkPadananPenyataId",
                table: "AkBankReconPenyataBank",
                column: "AkPadananPenyataId",
                principalTable: "AkPadananPenyata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
