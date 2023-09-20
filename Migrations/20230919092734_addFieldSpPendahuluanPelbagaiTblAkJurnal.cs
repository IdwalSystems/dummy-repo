using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldSpPendahuluanPelbagaiTblAkJurnal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpPendahuluanPelbagaiId",
                table: "AkJurnal",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "AkBank",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_SpPendahuluanPelbagaiId",
                table: "AkJurnal",
                column: "SpPendahuluanPelbagaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkJurnal",
                column: "SpPendahuluanPelbagaiId",
                principalTable: "SpPendahuluanPelbagai",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal_SpPendahuluanPelbagai_SpPendahuluanPelbagaiId",
                table: "AkJurnal");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal_SpPendahuluanPelbagaiId",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "SpPendahuluanPelbagaiId",
                table: "AkJurnal");

            migrationBuilder.AlterColumn<string>(
                name: "Kod",
                table: "AkBank",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
