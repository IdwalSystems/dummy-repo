using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNKTest.Migrations
{
    public partial class AddCaraBayar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaraBayar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KOD = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    PERIHAL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaraBayar", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaraBayar");
        }
    }
}
