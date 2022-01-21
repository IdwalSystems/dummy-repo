using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldsAkTunaiRuncitIdFlKategoriPenerimaNoRekupTblAkPV : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TarikhPosting",
                table: "AkTunaiCV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkTunaiRuncitId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlKategoriPenerima",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NoRekup",
                table: "AkPV",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_AkTunaiRuncitId",
                table: "AkPV",
                column: "AkTunaiRuncitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkPV",
                column: "AkTunaiRuncitId",
                principalTable: "AkTunaiRuncit",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_AkTunaiRuncit_AkTunaiRuncitId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_AkTunaiRuncitId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarikhPosting",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "AkTunaiRuncitId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "FlKategoriPenerima",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "NoRekup",
                table: "AkPV");
        }
    }
}
