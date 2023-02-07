using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJBahagianIdAkJurnal1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkJurnal1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAKB",
                table: "AkJurnal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianId",
                table: "AkJurnal1",
                column: "JBahagianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianId",
                table: "AkJurnal1",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "IsAKB",
                table: "AkJurnal");
        }
    }
}
