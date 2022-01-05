using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class MoveInsertRolesToSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
