using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldCompanyLogoWebPrintPDF : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoSyarikat",
                table: "SiAppInfo",
                newName: "CompanyLogoWeb");

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogoPrintPDF",
                table: "SiAppInfo",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLogoPrintPDF",
                table: "SiAppInfo");

            migrationBuilder.RenameColumn(
                name: "CompanyLogoWeb",
                table: "SiAppInfo",
                newName: "LogoSyarikat");
        }
    }
}
