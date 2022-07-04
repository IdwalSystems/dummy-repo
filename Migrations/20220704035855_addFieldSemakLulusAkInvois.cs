using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldSemakLulusAkInvois : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInvois",
                table: "JPenyemak",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsInvois",
                table: "JPelulus",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "FlCetak",
                table: "AkInvois",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusLulus",
                table: "AkInvois",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusSemak",
                table: "AkInvois",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JPelulusId",
                table: "AkInvois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JPenyemakId",
                table: "AkInvois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarLulus",
                table: "AkInvois",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarSemak",
                table: "AkInvois",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JPelulusId",
                table: "AkInvois",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkInvois_JPenyemakId",
                table: "AkInvois",
                column: "JPenyemakId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkInvois_JPelulus_JPelulusId",
                table: "AkInvois",
                column: "JPelulusId",
                principalTable: "JPelulus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkInvois_JPenyemak_JPenyemakId",
                table: "AkInvois",
                column: "JPenyemakId",
                principalTable: "JPenyemak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkInvois_JPelulus_JPelulusId",
                table: "AkInvois");

            migrationBuilder.DropForeignKey(
                name: "FK_AkInvois_JPenyemak_JPenyemakId",
                table: "AkInvois");

            migrationBuilder.DropIndex(
                name: "IX_AkInvois_JPelulusId",
                table: "AkInvois");

            migrationBuilder.DropIndex(
                name: "IX_AkInvois_JPenyemakId",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "IsInvois",
                table: "JPenyemak");

            migrationBuilder.DropColumn(
                name: "IsInvois",
                table: "JPelulus");

            migrationBuilder.DropColumn(
                name: "FlCetak",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "FlStatusLulus",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "FlStatusSemak",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "JPelulusId",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "JPenyemakId",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "TarLulus",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "TarSemak",
                table: "AkInvois");
        }
    }
}
