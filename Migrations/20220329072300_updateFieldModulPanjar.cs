using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class updateFieldModulPanjar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HadMaksimum",
                table: "AkTunaiRuncit",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "AkTunaiLejar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NoKP",
                table: "AkTunaiCV",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBajet",
                table: "AkCarta",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HadMaksimum",
                table: "AkTunaiRuncit");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "AkTunaiLejar");

            migrationBuilder.DropColumn(
                name: "NoKP",
                table: "AkTunaiCV");

            migrationBuilder.DropColumn(
                name: "IsBajet",
                table: "AkCarta");
        }
    }
}
