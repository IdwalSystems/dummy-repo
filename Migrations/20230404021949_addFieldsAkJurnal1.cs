using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldsAkJurnal1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaId",
                table: "AkJurnal1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_AkCartaId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "AkCartaId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "Debit",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "JBahagianId",
                table: "AkJurnal1");

            migrationBuilder.RenameColumn(
                name: "Kredit",
                table: "AkJurnal1",
                newName: "Amaun");

            migrationBuilder.AddColumn<int>(
                name: "AkCartaDebitId",
                table: "AkJurnal1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AkCartaKreditId",
                table: "AkJurnal1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianDebitId",
                table: "AkJurnal1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianKreditId",
                table: "AkJurnal1",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkCartaDebitId",
                table: "AkJurnal1",
                column: "AkCartaDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkCartaKreditId",
                table: "AkJurnal1",
                column: "AkCartaKreditId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianDebitId",
                table: "AkJurnal1",
                column: "JBahagianDebitId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianKreditId",
                table: "AkJurnal1",
                column: "JBahagianKreditId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaDebitId",
                table: "AkJurnal1",
                column: "AkCartaDebitId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaKreditId",
                table: "AkJurnal1",
                column: "AkCartaKreditId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianDebitId",
                table: "AkJurnal1",
                column: "JBahagianDebitId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianKreditId",
                table: "AkJurnal1",
                column: "JBahagianKreditId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaKreditId",
                table: "AkJurnal1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianKreditId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_AkCartaDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_AkCartaKreditId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_JBahagianDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropIndex(
                name: "IX_AkJurnal1_JBahagianKreditId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "AkCartaDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "AkCartaKreditId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "JBahagianDebitId",
                table: "AkJurnal1");

            migrationBuilder.DropColumn(
                name: "JBahagianKreditId",
                table: "AkJurnal1");

            migrationBuilder.RenameColumn(
                name: "Amaun",
                table: "AkJurnal1",
                newName: "Kredit");

            migrationBuilder.AddColumn<int>(
                name: "AkCartaId",
                table: "AkJurnal1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Debit",
                table: "AkJurnal1",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "JBahagianId",
                table: "AkJurnal1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_AkCartaId",
                table: "AkJurnal1",
                column: "AkCartaId");

            migrationBuilder.CreateIndex(
                name: "IX_AkJurnal1_JBahagianId",
                table: "AkJurnal1",
                column: "JBahagianId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_AkCarta_AkCartaId",
                table: "AkJurnal1",
                column: "AkCartaId",
                principalTable: "AkCarta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AkJurnal1_JBahagian_JBahagianId",
                table: "AkJurnal1",
                column: "JBahagianId",
                principalTable: "JBahagian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
