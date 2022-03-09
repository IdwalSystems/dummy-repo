using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJBahagianToAllTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkTunaiLejar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkPOLaras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkPO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkNotaMinta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkJurnal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkBelian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkBank",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkAkaun",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AbBukuVot",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JBahagianId",
                table: "SpPendahuluanPelbagai",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiRuncit_JBahagianId",
                table: "AkTunaiRuncit",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTunaiLejar_JBahagianId",
                table: "AkTunaiLejar",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_JBahagianId",
                table: "AkTerima",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JBahagianId",
                table: "AkPV",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPOLaras_JBahagianId",
                table: "AkPOLaras",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPO_JBahagianId",
                table: "AkPO",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JBahagianId",
                table: "AkNotaMinta",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal_JBahagianId",
                table: "AkJurnal",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBelian_JBahagianId",
                table: "AkBelian",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkBank_JBahagianId",
                table: "AkBank",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AkAkaun_JBahagianId",
                table: "AkAkaun",
                column: "JBahagianId");

            migrationBuilder.CreateIndex(
                name: "IX_AbBukuVot_JBahagianId",
                table: "AbBukuVot",
                column: "JBahagianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbBukuVot_JBahagian_JBahagianId",
                table: "AbBukuVot",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkAkaun_JBahagian_JBahagianId",
                table: "AkAkaun",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkBank_JBahagian_JBahagianId",
                table: "AkBank",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_JBahagian_JBahagianId",
                table: "AkBelian",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal_JBahagian_JBahagianId",
                table: "AkJurnal",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkNotaMinta_JBahagian_JBahagianId",
                table: "AkNotaMinta",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPO_JBahagian_JBahagianId",
                table: "AkPO",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPOLaras_JBahagian_JBahagianId",
                table: "AkPOLaras",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JBahagian_JBahagianId",
                table: "AkPV",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTerima_JBahagian_JBahagianId",
                table: "AkTerima",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiLejar_JBahagian_JBahagianId",
                table: "AkTunaiLejar",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkTunaiRuncit_JBahagian_JBahagianId",
                table: "AkTunaiRuncit",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_JBahagian_JBahagianId",
                table: "SpPendahuluanPelbagai",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbBukuVot_JBahagian_JBahagianId",
                table: "AbBukuVot");

            migrationBuilder.DropForeignKey(
                name: "FK_AkAkaun_JBahagian_JBahagianId",
                table: "AkAkaun");

            migrationBuilder.DropForeignKey(
                name: "FK_AkBank_JBahagian_JBahagianId",
                table: "AkBank");

            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_JBahagian_JBahagianId",
                table: "AkBelian");

            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal_JBahagian_JBahagianId",
                table: "AkJurnal");

            migrationBuilder.DropForeignKey(
                name: "FK_AkNotaMinta_JBahagian_JBahagianId",
                table: "AkNotaMinta");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPO_JBahagian_JBahagianId",
                table: "AkPO");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPOLaras_JBahagian_JBahagianId",
                table: "AkPOLaras");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JBahagian_JBahagianId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima_JBahagian_JBahagianId",
                table: "AkTerima");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiLejar_JBahagian_JBahagianId",
                table: "AkTunaiLejar");

            migrationBuilder.DropForeignKey(
                name: "FK_AkTunaiRuncit_JBahagian_JBahagianId",
                table: "AkTunaiRuncit");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_JBahagian_JBahagianId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai_JBahagianId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiRuncit_JBahagianId",
                table: "AkTunaiRuncit");

            migrationBuilder.DropIndex(
                name: "IX_AkTunaiLejar_JBahagianId",
                table: "AkTunaiLejar");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima_JBahagianId",
                table: "AkTerima");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_JBahagianId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkPOLaras_JBahagianId",
                table: "AkPOLaras");

            migrationBuilder.DropIndex(
                name: "IX_AkPO_JBahagianId",
                table: "AkPO");

            migrationBuilder.DropIndex(
                name: "IX_AkNotaMinta_JBahagianId",
                table: "AkNotaMinta");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal_JBahagianId",
                table: "AkJurnal");

            migrationBuilder.DropIndex(
                name: "IX_AkBelian_JBahagianId",
                table: "AkBelian");

            migrationBuilder.DropIndex(
                name: "IX_AkBank_JBahagianId",
                table: "AkBank");

            migrationBuilder.DropIndex(
                name: "IX_AkAkaun_JBahagianId",
                table: "AkAkaun");

            migrationBuilder.DropIndex(
                name: "IX_AbBukuVot_JBahagianId",
                table: "AbBukuVot");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkTunaiLejar");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AbBukuVot");
        }
    }
}
