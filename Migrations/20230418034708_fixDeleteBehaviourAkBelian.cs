using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class fixDeleteBehaviourAkBelian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_AkPO_AkPOId",
                table: "AkBelian");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_AkPO_AkPOId",
                table: "AkBelian",
                column: "AkPOId",
                principalTable: "AkPO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_AkPO_AkPOId",
                table: "AkBelian");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_AkPO_AkPOId",
                table: "AkBelian",
                column: "AkPOId",
                principalTable: "AkPO",
                principalColumn: "Id");
        }
    }
}
