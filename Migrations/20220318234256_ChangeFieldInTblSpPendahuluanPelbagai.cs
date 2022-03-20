using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class ChangeFieldInTblSpPendahuluanPelbagai : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatusSokong",
                table: "SpPendahuluanPelbagai",
                newName: "FlStatusSokong");

            migrationBuilder.RenameColumn(
                name: "StatusLulus",
                table: "SpPendahuluanPelbagai",
                newName: "FlStatusLulus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarSokong",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarSedia",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarLulus",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FlStatusSokong",
                table: "SpPendahuluanPelbagai",
                newName: "StatusSokong");

            migrationBuilder.RenameColumn(
                name: "FlStatusLulus",
                table: "SpPendahuluanPelbagai",
                newName: "StatusLulus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarSokong",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarSedia",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TarLulus",
                table: "SpPendahuluanPelbagai",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
