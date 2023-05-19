using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class addFieldIsLaporanBukuVotJPenyemakJPelulus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLaporanBukuVot",
                table: "JPenyemak",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLaporanBukuVot",
                table: "JPelulus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLaporanBukuVot",
                table: "JPenyemak");

            migrationBuilder.DropColumn(
                name: "IsLaporanBukuVot",
                table: "JPelulus");
        }
    }
}
