using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addfieldSebabHapusISoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "SuProfil",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "SuPekerja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "SuJurulatih",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "SuAtlet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "SpPendahuluanPelbagai",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JTahapAktiviti",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JSukan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JPTJ",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JProfilKategori",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JPenyemak",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JPelulus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JParas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JNegeri",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JKW",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JJenis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JJantina",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JCaraBayar",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JBank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JBangsa",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JBahagian",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "JAgama",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkTunaiRuncit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkTunaiPemegang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkTunaiCV",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkTerima",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPV",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPOLaras",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPO",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPenyataPemungut",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPenghutang",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkPembekal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkNotaMinta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkNotaDebitKreditBelian",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkJurnal",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkInvois",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkInden",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkCimbEFT",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkCarta",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkBelian",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkBankRecon",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AkBank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SebabHapus",
                table: "AbWaran",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "SuProfil");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "SuAtlet");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JPTJ");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JProfilKategori");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JPenyemak");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JPelulus");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JNegeri");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JKW");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JJantina");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JCaraBayar");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JBank");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JBangsa");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JBahagian");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "JAgama");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPenyataPemungut");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPenghutang");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkPembekal");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkNotaDebitKreditBelian");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkInden");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkCimbEFT");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkCarta");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkBankRecon");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "SebabHapus",
                table: "AbWaran");
        }
    }
}
