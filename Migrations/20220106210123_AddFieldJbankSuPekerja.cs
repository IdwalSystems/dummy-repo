using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldJbankSuPekerja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JBankId",
                table: "SuPekerja",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuPekerja_JBankId",
                table: "SuPekerja",
                column: "JBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja");

            migrationBuilder.DropIndex(
                name: "IX_SuPekerja_JBankId",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "JBankId",
                table: "SuPekerja");
        }
    }
}
