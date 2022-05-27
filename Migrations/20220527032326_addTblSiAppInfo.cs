using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addTblSiAppInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "SuPekerja",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SiAppInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodSistem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarVersi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NamaSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoPendaftaran = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlamatSyarikat1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlamatSyarikat2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlamatSyarikat3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bandar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poskod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Daerah = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Negeri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaksSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmelSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TarMula = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogoSyarikat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiAppInfo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiAppInfo");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "SuPekerja");
        }
    }
}
