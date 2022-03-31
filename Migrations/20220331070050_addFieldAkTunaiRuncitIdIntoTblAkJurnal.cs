using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldAkTunaiRuncitIdIntoTblAkJurnal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsInKewangan",
                table: "AkPO",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkTunaiRuncitId",
                table: "AkJurnal",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBajet",
                table: "AkCarta",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_AkTunaiRuncitId",
                table: "AkJurnal",
                column: "AkTunaiRuncitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkJurnal",
                column: "AkTunaiRuncitId",
                principalTable: "AkTunaiRuncit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkJurnal");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal_AkTunaiRuncitId",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "AkTunaiRuncitId",
                table: "AkJurnal");

            migrationBuilder.AlterColumn<bool>(
                name: "IsInKewangan",
                table: "AkPO",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsBajet",
                table: "AkCarta",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}
