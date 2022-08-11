using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addSuPekerjaIdSuPekerjaKemaskiniId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "SuProfil",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "SuProfil",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "SuPekerja",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "SuPekerja",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "SuJurulatih",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "SuJurulatih",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "SuAtlet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "SuAtlet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "SpPendahuluanPelbagai",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JTahapAktiviti",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JTahapAktiviti",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JSukan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JSukan",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JProfilKategori",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JProfilKategori",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JPenyemak",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JPenyemak",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JPelulus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JPelulus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JParas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JParas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JNegeri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JNegeri",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JKW",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JKW",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JJenis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JJenis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JJantina",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JJantina",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JCaraBayar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JCaraBayar",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JBank",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JBank",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JBangsa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JBangsa",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JBahagian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JBahagian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "JAgama",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "JAgama",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaId",
                table: "AppLog",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkTunaiRuncit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiPemegang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkTunaiPemegang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiCV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkTunaiCV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPOLaras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPOLaras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPO",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPenyataPemungut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPenyataPemungut",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPenghutang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPenghutang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkPembekal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkPembekal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkNotaMinta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkNotaMinta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkJurnal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkJurnal",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkInvois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkInvois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkInden",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkInden",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkCimbEFT",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkCimbEFT",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkCarta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkCarta",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkBelian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkBelian",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AkBank",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AkBank",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaKemaskiniId",
                table: "AbWaran",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaMasukId",
                table: "AbWaran",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "SuProfil");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "SuProfil");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "SuPekerja");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "SuJurulatih");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "SuAtlet");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "SuAtlet");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "SpPendahuluanPelbagai");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JTahapAktiviti");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JSukan");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JProfilKategori");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JProfilKategori");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JPenyemak");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JPenyemak");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JPelulus");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JPelulus");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JParas");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JNegeri");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JNegeri");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JKW");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JKW");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JJenis");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JJantina");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JJantina");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JCaraBayar");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JCaraBayar");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JBank");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JBank");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JBangsa");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JBangsa");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JBahagian");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JBahagian");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "JAgama");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "JAgama");

            migrationBuilder.DropColumn(
                name: "SuPekerjaId",
                table: "AppLog");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkTunaiPemegang");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkTerima");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPV");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPOLaras");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPO");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPenyataPemungut");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPenyataPemungut");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPenghutang");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPenghutang");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkPembekal");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkPembekal");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkNotaMinta");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkJurnal");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkInvois");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkInden");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkInden");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkCimbEFT");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkCimbEFT");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkCarta");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkCarta");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "SuPekerjaKemaskiniId",
                table: "AbWaran");

            migrationBuilder.DropColumn(
                name: "SuPekerjaMasukId",
                table: "AbWaran");
        }
    }
}
