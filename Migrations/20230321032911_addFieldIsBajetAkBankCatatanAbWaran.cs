using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldIsBajetAkBankCatatanAbWaran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBajet",
                table: "AkBank",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Catatan",
                table: "AbWaran",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBajet",
                table: "AkBank");

            migrationBuilder.DropColumn(
                name: "Catatan",
                table: "AbWaran");
        }
    }
}
