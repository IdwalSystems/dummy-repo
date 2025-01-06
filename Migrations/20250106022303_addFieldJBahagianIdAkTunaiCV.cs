using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJBahagianIdAkTunaiCV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuiti_JKonfigPerubahanEkuitiId",
                table: "JKonfigPerubahanEkuitiBaris");

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkTunaiCV",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiCV_JBahagianId",
                table: "AkTunaiCV",
                column: "JBahagianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiCV_JBahagian_JBahagianId",
                table: "AkTunaiCV",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuiti_JKonfigPerubahanEkuitiId",
                table: "JKonfigPerubahanEkuitiBaris",
                column: "JKonfigPerubahanEkuitiId",
                principalTable: "JKonfigPerubahanEkuiti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiCV_JBahagian_JBahagianId",
                table: "AkTunaiCV");

            migrationBuilder.DropForeignKey(
                name: "FK_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuiti_JKonfigPerubahanEkuitiId",
                table: "JKonfigPerubahanEkuitiBaris");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiCV_JBahagianId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkTunaiCV");

            migrationBuilder.AddForeignKey(
                name: "FK_JKonfigPerubahanEkuitiBaris_JKonfigPerubahanEkuiti_JKonfigPerubahanEkuitiId",
                table: "JKonfigPerubahanEkuitiBaris",
                column: "JKonfigPerubahanEkuitiId",
                principalTable: "JKonfigPerubahanEkuiti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
