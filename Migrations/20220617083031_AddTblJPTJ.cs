using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddTblJPTJ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JPTJId",
                table: "JBahagian",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JPTJ",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Perihal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JKWId = table.Column<int>(type: "int", nullable: true),
                    FlHapus = table.Column<int>(type: "int", nullable: false),
                    TarHapus = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JPTJ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JPTJ_JKW_JKWId",
                        column: x => x.JKWId,
                        principalTable: "JKW",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JBahagian_JPTJId",
                table: "JBahagian",
                column: "JPTJId");

            migrationBuilder.CreateIndex(
                name: "IX_JPTJ_JKWId",
                table: "JPTJ",
                column: "JKWId");

            migrationBuilder.AddForeignKey(
                name: "FK_JBahagian_JPTJ_JPTJId",
                table: "JBahagian",
                column: "JPTJId",
                principalTable: "JPTJ",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JBahagian_JPTJ_JPTJId",
                table: "JBahagian");

            migrationBuilder.DropTable(
                name: "JPTJ");

            migrationBuilder.DropIndex(
                name: "IX_JBahagian_JPTJId",
                table: "JBahagian");

            migrationBuilder.DropColumn(
                name: "JPTJId",
                table: "JBahagian");
        }
    }
}
