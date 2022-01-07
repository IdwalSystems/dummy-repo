using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class abBukuVot_Vot_ke_VotId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vot",
                table: "AbBukuVot",
                newName: "VotId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VotId",
                table: "AbBukuVot",
                newName: "Vot");
        }
    }
}
