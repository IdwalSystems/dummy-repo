using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class ubahSpPendahuluan1dan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai2_JJantina_JJantinaId",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai2_JJantinaId",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "BilAtl",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "BilJul",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "BilPeg",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "BilTek",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "BilUru",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "JJantinaId",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "JumL",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.RenameColumn(
                name: "JumP",
                table: "SpPendahuluanPelbagai2",
                newName: "Indek");

            migrationBuilder.RenameColumn(
                name: "JumL",
                table: "SpPendahuluanPelbagai2",
                newName: "Baris");

            migrationBuilder.RenameColumn(
                name: "JumP",
                table: "SpPendahuluanPelbagai1",
                newName: "Jumlah");

            migrationBuilder.AddColumn<decimal>(
                name: "Bil",
                table: "SpPendahuluanPelbagai2",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Bulan",
                table: "SpPendahuluanPelbagai2",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Jumlah",
                table: "SpPendahuluanPelbagai2",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Kadar",
                table: "SpPendahuluanPelbagai2",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Perihal",
                table: "SpPendahuluanPelbagai2",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bil",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "Bulan",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "Jumlah",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "Kadar",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.DropColumn(
                name: "Perihal",
                table: "SpPendahuluanPelbagai2");

            migrationBuilder.RenameColumn(
                name: "Indek",
                table: "SpPendahuluanPelbagai2",
                newName: "JumP");

            migrationBuilder.RenameColumn(
                name: "Baris",
                table: "SpPendahuluanPelbagai2",
                newName: "JumL");

            migrationBuilder.RenameColumn(
                name: "Jumlah",
                table: "SpPendahuluanPelbagai1",
                newName: "JumP");

            migrationBuilder.AddColumn<int>(
                name: "BilAtl",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilJul",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilPeg",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilTek",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilUru",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JJantinaId",
                table: "SpPendahuluanPelbagai2",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumL",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai2_JJantinaId",
                table: "SpPendahuluanPelbagai2",
                column: "JJantinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai2_JJantina_JJantinaId",
                table: "SpPendahuluanPelbagai2",
                column: "JJantinaId",
                principalTable: "JJantina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
