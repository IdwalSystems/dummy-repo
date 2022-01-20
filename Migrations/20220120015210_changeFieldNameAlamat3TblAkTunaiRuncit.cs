using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class changeFieldNameAlamat3TblAkTunaiRuncit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Almat3",
                table: "AkTunaiCV",
                newName: "Alamat3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Alamat3",
                table: "AkTunaiCV",
                newName: "Almat3");
        }
    }
}
