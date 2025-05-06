using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldAmaunTetapJKonfigPenyataBarisFormula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmaunTetap",
                table: "JKonfigPenyataBarisFormula",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsLastYear",
                table: "JKonfigPenyataBarisFormula",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUntilYear",
                table: "JKonfigPenyataBarisFormula",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmaunTetap",
                table: "JKonfigPenyataBarisFormula");

            migrationBuilder.DropColumn(
                name: "IsLastYear",
                table: "JKonfigPenyataBarisFormula");

            migrationBuilder.DropColumn(
                name: "IsUntilYear",
                table: "JKonfigPenyataBarisFormula");
        }
    }
}
