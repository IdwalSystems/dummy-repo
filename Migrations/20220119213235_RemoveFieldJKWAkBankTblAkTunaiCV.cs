using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class RemoveFieldJKWAkBankTblAkTunaiCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_AkBank_AkBankId",
                table: "AkTunaiCV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_JKW_JKWId",
                table: "AkTunaiCV");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_AkBankId",
                table: "AkTunaiCV");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_JKWId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "AkBankId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "JKWId",
                table: "AkTunaiCV");

            migrationBuilder.AlterColumn<string>(
                name: "Catatan1",
                table: "AkJurnal",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldNullable: true,
                oldDefaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkBankId",
                table: "AkTunaiCV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JKWId",
                table: "AkTunaiCV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Catatan1",
                table: "AkJurnal",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(400)",
                oldMaxLength: 400,
                oldDefaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_AkBankId",
                table: "AkTunaiCV",
                column: "AkBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_JKWId",
                table: "AkTunaiCV",
                column: "JKWId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_AkBank_AkBankId",
                table: "AkTunaiCV",
                column: "AkBankId",
                principalTable: "AkBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_JKW_JKWId",
                table: "AkTunaiCV",
                column: "JKWId",
                principalTable: "JKW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
