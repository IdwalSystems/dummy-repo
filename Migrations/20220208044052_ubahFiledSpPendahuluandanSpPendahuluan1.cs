using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class ubahFiledSpPendahuluandanSpPendahuluan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai1_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai1_AkCartaId",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "Bln",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "Jumlah",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "Kadar",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "Perihal",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "JTahapId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JumAtl",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JumJul",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JumPeg",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "JumTek",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.RenameColumn(
                name: "Bil",
                table: "SpPendahuluanPelbagai1",
                newName: "JumP");

            migrationBuilder.RenameColumn(
                name: "AkCartaId",
                table: "SpPendahuluanPelbagai1",
                newName: "JumL");

            migrationBuilder.RenameColumn(
                name: "JumUru",
                table: "SpPendahuluanPelbagai",
                newName: "AkCartaId");

            migrationBuilder.AddColumn<int>(
                name: "BilAtl",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilJul",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilPeg",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilTek",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BilUru",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JJantinaId",
                table: "SpPendahuluanPelbagai1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_JJantinaId",
                table: "SpPendahuluanPelbagai1",
                column: "JJantinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai_AkCartaId",
                table: "SpPendahuluanPelbagai",
                column: "AkCartaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                column: "JTahapAktivitiId",
                principalTable: "JTahapAktiviti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai1_JJantina_JJantinaId",
                table: "SpPendahuluanPelbagai1",
                column: "JJantinaId",
                principalTable: "JJantina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropForeignKey(
                name: "FK_SpPendahuluanPelbagai1_JJantina_JJantinaId",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai1_JJantinaId",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropIndex(
                name: "IX_SpPendahuluanPelbagai_AkCartaId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "BilAtl",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "BilJul",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "BilPeg",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "BilTek",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "BilUru",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.DropColumn(
                name: "JJantinaId",
                table: "SpPendahuluanPelbagai1");

            migrationBuilder.RenameColumn(
                name: "JumP",
                table: "SpPendahuluanPelbagai1",
                newName: "Bil");

            migrationBuilder.RenameColumn(
                name: "JumL",
                table: "SpPendahuluanPelbagai1",
                newName: "AkCartaId");

            migrationBuilder.RenameColumn(
                name: "AkCartaId",
                table: "SpPendahuluanPelbagai",
                newName: "JumUru");

            migrationBuilder.AddColumn<decimal>(
                name: "Bln",
                table: "SpPendahuluanPelbagai1",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Jumlah",
                table: "SpPendahuluanPelbagai1",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Kadar",
                table: "SpPendahuluanPelbagai1",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Perihal",
                table: "SpPendahuluanPelbagai1",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "JTahapId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumAtl",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumJul",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumPeg",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JumTek",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpPendahuluanPelbagai1_AkCartaId",
                table: "SpPendahuluanPelbagai1",
                column: "AkCartaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai_JTahapAktiviti_JTahapAktivitiId",
                table: "SpPendahuluanPelbagai",
                column: "JTahapAktivitiId",
                principalTable: "JTahapAktiviti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SpPendahuluanPelbagai1_AkCarta_AkCartaId",
                table: "SpPendahuluanPelbagai1",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
