using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldFlMaintainanceTblSiAppInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlMaintainance",
                table: "SiAppInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlMaintainance",
                table: "SiAppInfo");
        }
    }
}
