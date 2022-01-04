using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class update_supekerja_tambah_nokp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f24d001-e893-491e-bbc1-974d2ee2e0f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e2dea98-a0ed-4120-979f-4df7340fbcc7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd746c98-eba5-4f9c-a60e-2f062602ddf9");

            migrationBuilder.AddColumn<string>(
                name: "NoKp",
                table: "SuPekerja",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "49540456-080b-4acd-be1e-74fcaf94f823", "b4ef8452-c95d-464c-a40a-d0246f6b546c", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "da823e26-06de-445a-9f52-c54fabef4457", "bfad193f-9f67-4947-aca4-84ad0f0689fa", "Supervisor", "SUPERVISOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bfeb4f6f-0987-4178-9635-dc7e36e4f632", "04ef121a-7dbb-44ec-8e74-97ad3763d905", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49540456-080b-4acd-be1e-74fcaf94f823");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bfeb4f6f-0987-4178-9635-dc7e36e4f632");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "da823e26-06de-445a-9f52-c54fabef4457");

            migrationBuilder.DropColumn(
                name: "NoKp",
                table: "SuPekerja");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1f24d001-e893-491e-bbc1-974d2ee2e0f1", "b6a9368b-c1c6-42a0-911f-b1de0ef975f9", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "bd746c98-eba5-4f9c-a60e-2f062602ddf9", "896f7297-d3a6-4a1a-a1a3-1c3d8e3d03a3", "Supervisor", "SUPERVISOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5e2dea98-a0ed-4120-979f-4df7340fbcc7", "8277092e-c8d0-4b2c-a749-860517dc928c", "User", "USER" });
        }
    }
}
