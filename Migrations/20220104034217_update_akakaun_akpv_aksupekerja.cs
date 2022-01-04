using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class update_akakaun_akpv_aksupekerja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkTerima_AkAkaun_AkAkaunId",
                table: "AkTerima");

            migrationBuilder.DropIndex(
                name: "IX_AkTerima_AkAkaunId",
                table: "AkTerima");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "06b9bbaf-3ca6-4349-b7dc-959d57cd6228");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75bec53c-f5ed-4628-80f8-76f13a86b3ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b362e175-f6cb-4ad5-94fb-194919a1d85b");

            migrationBuilder.DropColumn(
                name: "AkAkaunId",
                table: "AkTerima");

            migrationBuilder.AddColumn<int>(
                name: "SuPekerjaId",
                table: "AkPV",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5698d4e4-dff3-401b-985c-f98110013712", "bfc43065-8e45-4059-bc86-8356d4ff6fe1", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8d84f4b3-f23c-4821-8679-67b90246906a", "5a04a28d-38d4-45c9-801c-c15847a1dd90", "Supervisor", "SUPERVISOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8d516f54-d94c-4554-8052-fa79d0e26318", "c7feb179-01a4-4257-9705-c77139149d67", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_AkPV_SuPekerjaId",
                table: "AkPV",
                column: "SuPekerjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkPV_SuPekerja_SuPekerjaId",
                table: "AkPV",
                column: "SuPekerjaId",
                principalTable: "SuPekerja",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AkPV_SuPekerja_SuPekerjaId",
                table: "AkPV");

            migrationBuilder.DropIndex(
                name: "IX_AkPV_SuPekerjaId",
                table: "AkPV");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5698d4e4-dff3-401b-985c-f98110013712");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d516f54-d94c-4554-8052-fa79d0e26318");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d84f4b3-f23c-4821-8679-67b90246906a");

            migrationBuilder.DropColumn(
                name: "SuPekerjaId",
                table: "AkPV");

            migrationBuilder.AddColumn<int>(
                name: "AkAkaunId",
                table: "AkTerima",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "06b9bbaf-3ca6-4349-b7dc-959d57cd6228", "3da568ce-0cab-4a66-9e9e-77c533e20307", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "75bec53c-f5ed-4628-80f8-76f13a86b3ba", "5bc28f35-6775-4ab9-be59-1b6c111d312c", "Supervisor", "SUPERVISOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b362e175-f6cb-4ad5-94fb-194919a1d85b", "bfa90a60-ebcf-4c3f-af9e-1f9541a4d740", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_AkTerima_AkAkaunId",
                table: "AkTerima",
                column: "AkAkaunId");

            migrationBuilder.AddForeignKey(
                name: "FK_AkTerima_AkAkaun_AkAkaunId",
                table: "AkTerima",
                column: "AkAkaunId",
                principalTable: "AkAkaun",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
