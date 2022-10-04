using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class AddFieldAkPVGandaIdTblAkPadananPenyata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AkPVGandaId",
                table: "AkPadananPenyata",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AkPVGandaAkPadananPenyata",
                columns: table => new
                {
                    AkPVGandaId = table.Column<int>(type: "int", nullable: false),
                    AkPadananPenyataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AkPVGandaAkPadananPenyata", x => new { x.AkPVGandaId, x.AkPadananPenyataId });
                    table.ForeignKey(
                        name: "FK_AkPVGandaAkPadananPenyata_AkPadananPenyata_AkPadananPenyataId",
                        column: x => x.AkPadananPenyataId,
                        principalTable: "AkPadananPenyata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AkPVGandaAkPadananPenyata_AkPVGanda_AkPVGandaId",
                        column: x => x.AkPVGandaId,
                        principalTable: "AkPVGanda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AkPVGandaAkPadananPenyata_AkPadananPenyataId",
                table: "AkPVGandaAkPadananPenyata",
                column: "AkPadananPenyataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AkPVGandaAkPadananPenyata");

            migrationBuilder.DropColumn(
                name: "AkPVGandaId",
                table: "AkPadananPenyata");
        }
    }
}
