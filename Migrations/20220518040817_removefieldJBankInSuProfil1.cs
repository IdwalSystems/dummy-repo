using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class removefieldJBankInSuProfil1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuProfil1_JBank_JBankId",
                table: "SuProfil1");

            migrationBuilder.DropIndex(
                name: "IX_SuProfil1_JBankId",
                table: "SuProfil1");

            migrationBuilder.DropColumn(
                name: "JBankId",
                table: "SuProfil1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JBankId",
                table: "SuProfil1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SuProfil1_JBankId",
                table: "SuProfil1",
                column: "JBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuProfil1_JBank_JBankId",
                table: "SuProfil1",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
