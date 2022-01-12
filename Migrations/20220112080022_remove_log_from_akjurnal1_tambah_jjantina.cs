using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class remove_log_from_akjurnal1_tambah_jjantina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TarKemaskini",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "TarMasuk",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "UserIdKemaskini",
                table: "AkJurnal1");

            migrationBuilder.CreateTable(
                name: "JJantina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Perihal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMasuk = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIdKemaskini = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarKemaskini = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JJantina", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JJantina");

            migrationBuilder.AddColumn<DateTime>(
                name: "TarKemaskini",
                table: "AkJurnal1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TarMasuk",
                table: "AkJurnal1",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AkJurnal1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdKemaskini",
                table: "AkJurnal1",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
