using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class ChangeFieldTblAkBelian_AddFieldsTblAkAkaun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_AkBank_AkBankId",
                table: "AkBelian");

            migrationBuilder.RenameColumn(
                name: "AkBankId",
                table: "AkBelian",
                newName: "KodObjekAPId");

            migrationBuilder.RenameIndex(
                name: "IX_AkBelian_AkBankId",
                table: "AkBelian",
                newName: "IX_AkBelian_KodObjekAPId");

            migrationBuilder.AddColumn<string>(
                name: "Bulan",
                table: "AkAkaun",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ganding",
                table: "AkAkaun",
                type: "int",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoCek",
                table: "AkAkaun",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoSlip",
                table: "AkAkaun",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tahun",
                table: "AkAkaun",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarSlip",
                table: "AkAkaun",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tunai",
                table: "AkAkaun",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_AkCarta_KodObjekAPId",
                table: "AkBelian",
                column: "KodObjekAPId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkBelian_AkCarta_KodObjekAPId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "Bulan",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "Ganding",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "NoCek",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "NoSlip",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "Tahun",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "TarSlip",
                table: "AkAkaun");

            migrationBuilder.DropColumn(
                name: "Tunai",
                table: "AkAkaun");

            migrationBuilder.RenameColumn(
                name: "KodObjekAPId",
                table: "AkBelian",
                newName: "AkBankId");

            migrationBuilder.RenameIndex(
                name: "IX_AkBelian_KodObjekAPId",
                table: "AkBelian",
                newName: "IX_AkBelian_AkBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkBelian_AkBank_AkBankId",
                table: "AkBelian",
                column: "AkBankId",
                principalTable: "AkBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
