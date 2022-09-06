using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class DetachJCarabayarFromAkPVGanda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JCaraBayar_JCaraBayarId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPVGanda_JBank_JBankId",
                table: "AkPVGanda");

            migrationBuilder.AlterColumn<int>(
                name: "JCaraBayarId",
                table: "AkPV",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JCaraBayar_JCaraBayarId",
                table: "AkPV",
                column: "JCaraBayarId",
                principalTable: "JCaraBayar",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPVGanda_JBank_JBankId",
                table: "AkPVGanda",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JCaraBayar_JCaraBayarId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPVGanda_JBank_JBankId",
                table: "AkPVGanda");

            migrationBuilder.AlterColumn<int>(
                name: "JCaraBayarId",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JCaraBayar_JCaraBayarId",
                table: "AkPV",
                column: "JCaraBayarId",
                principalTable: "JCaraBayar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPVGanda_JBank_JBankId",
                table: "AkPVGanda",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
