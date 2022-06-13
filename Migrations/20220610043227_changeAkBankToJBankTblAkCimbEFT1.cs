using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class changeAkBankToJBankTblAkCimbEFT1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_AkPembekal_PenerimaId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuAtlet_PenerimaId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuJurulatih_PenerimaId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuPekerja_PenerimaId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_SuAtlet_JBank_JBankId",
                table: "SuAtlet");

            migrationBuilder.DropForeignKey(
                name: "FK_SuJurulatih_JBank_JBankId",
                table: "SuJurulatih");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja");

            migrationBuilder.RenameColumn(
                name: "PenerimaId",
                table: "AkCimbEFT1",
                newName: "SuPekerjaId");

            migrationBuilder.RenameIndex(
                name: "IX_AkCimbEFT1_PenerimaId",
                table: "AkCimbEFT1",
                newName: "IX_AkCimbEFT1_SuPekerjaId");

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuPekerja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuJurulatih",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuAtlet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBankId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AkBankId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AkPembekalId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JBankId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SuAtletId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuJurulatihId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_JBankId",
                table: "AkPV",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_AkPembekalId",
                table: "AkCimbEFT1",
                column: "AkPembekalId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_JBankId",
                table: "AkCimbEFT1",
                column: "JBankId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_SuAtletId",
                table: "AkCimbEFT1",
                column: "SuAtletId");

            migrationBuilder.CreateIndex(
                name: "IX_AkCimbEFT1_SuJurulatihId",
                table: "AkCimbEFT1",
                column: "SuJurulatihId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_AkPembekal_AkPembekalId",
                table: "AkCimbEFT1",
                column: "AkPembekalId",
                principalTable: "AkPembekal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_JBank_JBankId",
                table: "AkCimbEFT1",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuAtlet_SuAtletId",
                table: "AkCimbEFT1",
                column: "SuAtletId",
                principalTable: "SuAtlet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuJurulatih_SuJurulatihId",
                table: "AkCimbEFT1",
                column: "SuJurulatihId",
                principalTable: "SuJurulatih",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuPekerja_SuPekerjaId",
                table: "AkCimbEFT1",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_JBank_JBankId",
                table: "AkPV",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuAtlet_JBank_JBankId",
                table: "SuAtlet",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuJurulatih_JBank_JBankId",
                table: "SuJurulatih",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_AkPembekal_AkPembekalId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_JBank_JBankId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuAtlet_SuAtletId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuJurulatih_SuJurulatihId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkCimbEFT1_SuPekerja_SuPekerjaId",
                table: "AkCimbEFT1");

            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_JBank_JBankId",
                table: "AkPV");

            migrationBuilder.DropForeignKey(
                name: "FK_SuAtlet_JBank_JBankId",
                table: "SuAtlet");

            migrationBuilder.DropForeignKey(
                name: "FK_SuJurulatih_JBank_JBankId",
                table: "SuJurulatih");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_JBankId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkCimbEFT1_AkPembekalId",
                table: "AkCimbEFT1");

            migrationBuilder.DropIndex(
                name: "IX_AkCimbEFT1_JBankId",
                table: "AkCimbEFT1");

            migrationBuilder.DropIndex(
                name: "IX_AkCimbEFT1_SuAtletId",
                table: "AkCimbEFT1");

            migrationBuilder.DropIndex(
                name: "IX_AkCimbEFT1_SuJurulatihId",
                table: "AkCimbEFT1");

            migrationBuilder.DropColumn(
                name: "JBankId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "AkPembekalId",
                table: "AkCimbEFT1");

            migrationBuilder.DropColumn(
                name: "JBankId",
                table: "AkCimbEFT1");

            migrationBuilder.DropColumn(
                name: "SuAtletId",
                table: "AkCimbEFT1");

            migrationBuilder.DropColumn(
                name: "SuJurulatihId",
                table: "AkCimbEFT1");

            migrationBuilder.RenameColumn(
                name: "SuPekerjaId",
                table: "AkCimbEFT1",
                newName: "PenerimaId");

            migrationBuilder.RenameIndex(
                name: "IX_AkCimbEFT1_SuPekerjaId",
                table: "AkCimbEFT1",
                newName: "IX_AkCimbEFT1_PenerimaId");

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuPekerja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuJurulatih",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JBankId",
                table: "SuAtlet",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AkBankId",
                table: "AkCimbEFT1",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_AkPembekal_PenerimaId",
                table: "AkCimbEFT1",
                column: "PenerimaId",
                principalTable: "AkPembekal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuAtlet_PenerimaId",
                table: "AkCimbEFT1",
                column: "PenerimaId",
                principalTable: "SuAtlet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuJurulatih_PenerimaId",
                table: "AkCimbEFT1",
                column: "PenerimaId",
                principalTable: "SuJurulatih",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AkCimbEFT1_SuPekerja_PenerimaId",
                table: "AkCimbEFT1",
                column: "PenerimaId",
                principalTable: "SuPekerja",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SuAtlet_JBank_JBankId",
                table: "SuAtlet",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuJurulatih_JBank_JBankId",
                table: "SuJurulatih",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JBank_JBankId",
                table: "SuPekerja",
                column: "JBankId",
                principalTable: "JBank",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
