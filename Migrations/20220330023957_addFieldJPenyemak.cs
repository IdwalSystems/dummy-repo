using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldJPenyemak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pelulus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "Penyokong",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.AddColumn<int>(
                name: "JPelulusId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JPenyemakId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusLulus",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusSemak",
                table: "AkPV",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JPelulusId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JPenyemakId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarLulus",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarSemak",
                table: "AkPV",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsInKewangan",
                table: "AkPO",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusLulus",
                table: "AkNotaMinta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlStatusSemak",
                table: "AkNotaMinta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JPelulusId",
                table: "AkNotaMinta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JPenyemakId",
                table: "AkNotaMinta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarLulus",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarSemak",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBajet",
                table: "AkCarta",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JPelulusId",
                table: "SpPendahuluanPelbagai",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_JPenyemakId",
                table: "SpPendahuluanPelbagai",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JPelulusId",
                table: "AkPV",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JPenyemakId",
                table: "AkPV",
                column: "JPenyemakId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JPelulusId",
                table: "AkNotaMinta",
                column: "JPelulusId");

            migrationBuilder.CreateIndex(
                name: "IX_AkNotaMinta_JPenyemakId",
                table: "AkNotaMinta",
                column: "JPenyemakId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkNotaMinta_JPelulus_JPelulusId",
                table: "AkNotaMinta",
                column: "JPelulusId",
                principalTable: "JPelulus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkNotaMinta_JPenyemak_JPenyemakId",
                table: "AkNotaMinta",
                column: "JPenyemakId",
                principalTable: "JPenyemak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JPelulus_JPelulusId",
                table: "AkPV",
                column: "JPelulusId",
                principalTable: "JPelulus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JPenyemak_JPenyemakId",
                table: "AkPV",
                column: "JPenyemakId",
                principalTable: "JPenyemak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_JPelulus_JPelulusId",
                table: "SpPendahuluanPelbagai",
                column: "JPelulusId",
                principalTable: "JPelulus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_JPenyemak_JPenyemakId",
                table: "SpPendahuluanPelbagai",
                column: "JPenyemakId",
                principalTable: "JPenyemak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkNotaMinta_JPelulus_JPelulusId",
                table: "AkNotaMinta");

            migrationBuilder.DropForeignKey(
                name: "FK_AkNotaMinta_JPenyemak_JPenyemakId",
                table: "AkNotaMinta");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JPelulus_JPelulusId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JPenyemak_JPenyemakId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_JPelulus_JPelulusId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_JPenyemak_JPenyemakId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai_JPelulusId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai_JPenyemakId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_JPelulusId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_JPenyemakId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkNotaMinta_JPelulusId",
                table: "AkNotaMinta");

            migrationBuilder.DropIndex(
                name: "IX_AkNotaMinta_JPenyemakId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "JPelulusId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JPenyemakId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "FlStatusLulus",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "FlStatusSemak",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "JPelulusId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "JPenyemakId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarLulus",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "TarSemak",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "IsInKewangan",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "FlStatusLulus",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "FlStatusSemak",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "JPelulusId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "JPenyemakId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "TarLulus",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "TarSemak",
                table: "AkNotaMinta");

            migrationBuilder.AddColumn<string>(
                name: "Pelulus",
                table: "SpPendahuluanPelbagai",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Penyokong",
                table: "SpPendahuluanPelbagai",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBajet",
                table: "AkCarta",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);
        }
    }
}
