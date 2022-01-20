using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class PerubahanFieldAktiviti_Akitiviti1_Aktiviti2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti1_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropColumn(
                name: "BilAtlL",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilAtlP",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilJulL",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilJulP",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilPegL",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilPegP",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilTekL",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilTekP",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "BilUruL",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.RenameColumn(
                name: "JumUru",
                table: "SpPermohonanAktiviti2",
                newName: "JJantinaId");

            migrationBuilder.RenameColumn(
                name: "JumTek",
                table: "SpPermohonanAktiviti2",
                newName: "BilUru");

            migrationBuilder.RenameColumn(
                name: "JumPeg",
                table: "SpPermohonanAktiviti2",
                newName: "BilTek");

            migrationBuilder.RenameColumn(
                name: "JumJul",
                table: "SpPermohonanAktiviti2",
                newName: "BilPeg");

            migrationBuilder.RenameColumn(
                name: "JumAtl",
                table: "SpPermohonanAktiviti2",
                newName: "BilJul");

            migrationBuilder.RenameColumn(
                name: "BilUruP",
                table: "SpPermohonanAktiviti2",
                newName: "BilAtl");

            migrationBuilder.AddColumn<decimal>(
                name: "Jumlah",
                table: "SpPermohonanAktiviti1",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "JKWId",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumAtl",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumJul",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumPeg",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumTek",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumUru",
                table: "SpPermohonanAktiviti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NoPermohonan",
                table: "SpPermohonanAktiviti",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_JJantinaId",
                table: "SpPermohonanAktiviti2",
                column: "JJantinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2",
                column: "SpPermohonanAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti1_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti1",
                column: "SpPermohonanAktivitiId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti_JKWId",
                table: "SpPermohonanAktiviti",
                column: "JKWId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpPermohonanAktiviti_JKW_JKWId",
                table: "SpPermohonanAktiviti",
                column: "JKWId",
                principalTable: "JKW",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPermohonanAktiviti2_JJantina_JJantinaId",
                table: "SpPermohonanAktiviti2",
                column: "JJantinaId",
                principalTable: "JJantina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPermohonanAktiviti_JKW_JKWId",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPermohonanAktiviti2_JJantina_JJantinaId",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti2_JJantinaId",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2");

            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti1_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropIndex(
                name: "IX_SpPermohonanAktiviti_JKWId",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "Jumlah",
                table: "SpPermohonanAktiviti1");

            migrationBuilder.DropColumn(
                name: "JKWId",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "JumAtl",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "JumJul",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "JumPeg",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "JumTek",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "JumUru",
                table: "SpPermohonanAktiviti");

            migrationBuilder.DropColumn(
                name: "NoPermohonan",
                table: "SpPermohonanAktiviti");

            migrationBuilder.RenameColumn(
                name: "JJantinaId",
                table: "SpPermohonanAktiviti2",
                newName: "JumUru");

            migrationBuilder.RenameColumn(
                name: "BilUru",
                table: "SpPermohonanAktiviti2",
                newName: "JumTek");

            migrationBuilder.RenameColumn(
                name: "BilTek",
                table: "SpPermohonanAktiviti2",
                newName: "JumPeg");

            migrationBuilder.RenameColumn(
                name: "BilPeg",
                table: "SpPermohonanAktiviti2",
                newName: "JumJul");

            migrationBuilder.RenameColumn(
                name: "BilJul",
                table: "SpPermohonanAktiviti2",
                newName: "JumAtl");

            migrationBuilder.RenameColumn(
                name: "BilAtl",
                table: "SpPermohonanAktiviti2",
                newName: "BilUruP");

            migrationBuilder.AddColumn<int>(
                name: "BilAtlL",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilAtlP",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilJulL",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilJulP",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilPegL",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilPegP",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilTekL",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilTekP",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilUruL",
                table: "SpPermohonanAktiviti2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "SpPermohonanAktiviti2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "SpPermohonanAktiviti2",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SpPermohonanAktiviti2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "SpPermohonanAktiviti2",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "SpPermohonanAktiviti1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "SpPermohonanAktiviti1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SpPermohonanAktiviti1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "SpPermohonanAktiviti1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti2_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti2",
                column: "SpPermohonanAktivitiId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPermohonanAktiviti1_SpPermohonanAktivitiId",
                table: "SpPermohonanAktiviti1",
                column: "SpPermohonanAktivitiId",
                unique: true);
        }
    }
}
