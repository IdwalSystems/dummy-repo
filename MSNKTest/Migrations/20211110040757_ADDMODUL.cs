using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNKTest.Migrations
{
    public partial class ADDMODUL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modul",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FUNCID = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FUNCNAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modul", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modul");
        }
    }
}
