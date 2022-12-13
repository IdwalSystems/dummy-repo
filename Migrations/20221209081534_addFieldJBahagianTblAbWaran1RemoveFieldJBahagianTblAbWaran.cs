using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJBahagianTblAbWaran1RemoveFieldJBahagianTblAbWaran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AbWaran1",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JBahagianId",
                table: "AbWaran",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FlJenisPindahan",
                table: "AbWaran",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AbWaran1_JBahagianId",
                table: "AbWaran1",
                column: "JBahagianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbWaran1_JBahagian_JBahagianId",
                table: "AbWaran1",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbWaran1_JBahagian_JBahagianId",
                table: "AbWaran1");

            migrationBuilder.DropIndex(
                name: "IX_AbWaran1_JBahagianId",
                table: "AbWaran1");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AbWaran1");

            migrationBuilder.DropColumn(
                name: "FlJenisPindahan",
                table: "AbWaran");

            migrationBuilder.AlterColumn<int>(
                name: "JBahagianId",
                table: "AbWaran",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
