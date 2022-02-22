using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFunctionSoftDeleteIntoTblJadualAndDaftar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "SuTanggunganPekerja");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "SuTanggunganPekerja");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SuTanggunganPekerja");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "SuTanggunganPekerja");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AbBukuVot");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AbBukuVot");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AbBukuVot");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AbBukuVot");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "SuPekerja",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "SuPekerja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "SuPekerja",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JTahapAktiviti",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JTahapAktiviti",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JTahapAktiviti",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JSukan",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JSukan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JSukan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JParas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JParas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "JParas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "JParas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JParas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "JParas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JNegeri",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JNegeri",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JNegeri",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JKW",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JKW",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JKW",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JJenis",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JJenis",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "JJenis",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "JJenis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JJenis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "JJenis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JJawatanPekerja",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JJawatanPekerja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JJawatanPekerja",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JJantina",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JJantina",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JJantina",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JCaraBayar",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JCaraBayar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JCaraBayar",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JBank",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JBank",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JBank",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JBangsa",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JBangsa",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JBangsa",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JAgama",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "JAgama",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "JAgama",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTunaiRuncit",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "AkTunaiPemegang",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkTunaiPemegang",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTunaiPemegang",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkTunaiPemegang",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkTunaiPemegang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkTunaiPemegang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTunaiCV",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTerima",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPV",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPOLaras",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPO",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPembekal",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "AkPembekal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkPembekal",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkJurnal",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkCarta",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "AkCarta",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkCarta",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBelian",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBank",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "AkBank",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "AkBank",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JNegeri");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JNegeri");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JKW");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JKW");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JJawatanPekerja");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JJawatanPekerja");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JJantina");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JJantina");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JCaraBayar");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JCaraBayar");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JBank");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JBank");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JBangsa");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JBangsa");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "JAgama");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "JAgama");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "AkPembekal");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkPembekal");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "AkCarta");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkCarta");

            migrationBuilder.DropColumn(
                name: "FlHapus",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "TarHapus",
                table: "AkBank");

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "SuTanggunganPekerja",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "SuTanggunganPekerja",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SuTanggunganPekerja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "SuTanggunganPekerja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "SuPekerja",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FlHapus",
                table: "SpPendahuluanPelbagai",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarHapus",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JTahapAktiviti",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JSukan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JNegeri",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JKW",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JJawatanPekerja",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JJantina",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JCaraBayar",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JBank",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JBangsa",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "JAgama",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTunaiRuncit",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTunaiCV",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkTerima",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPV",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPOLaras",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPO",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkPembekal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkNotaMinta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkJurnal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkCarta",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBelian",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkBank",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AbBukuVot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AbBukuVot",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AbBukuVot",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AbBukuVot",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
