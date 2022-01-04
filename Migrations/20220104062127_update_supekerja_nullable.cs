using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    public partial class update_supekerja_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JAgama_JAgamaId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JBangsa_JBangsaId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JCaraBayar_JCaraBayarId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja");

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

            migrationBuilder.AlterColumn<int>(
                name: "JJawatanPekerjaId",
                table: "SuPekerja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JCaraBayarId",
                table: "SuPekerja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JBangsaId",
                table: "SuPekerja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "JAgamaId",
                table: "SuPekerja",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JAgama_JAgamaId",
                table: "SuPekerja",
                column: "JAgamaId",
                principalTable: "JAgama",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JBangsa_JBangsaId",
                table: "SuPekerja",
                column: "JBangsaId",
                principalTable: "JBangsa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JCaraBayar_JCaraBayarId",
                table: "SuPekerja",
                column: "JCaraBayarId",
                principalTable: "JCaraBayar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja",
                column: "JJawatanPekerjaId",
                principalTable: "JJawatanPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JAgama_JAgamaId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JBangsa_JBangsaId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JCaraBayar_JCaraBayarId",
                table: "SuPekerja");

            migrationBuilder.DropForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja");

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

            migrationBuilder.AlterColumn<int>(
                name: "JJawatanPekerjaId",
                table: "SuPekerja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JCaraBayarId",
                table: "SuPekerja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JBangsaId",
                table: "SuPekerja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JAgamaId",
                table: "SuPekerja",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JAgama_JAgamaId",
                table: "SuPekerja",
                column: "JAgamaId",
                principalTable: "JAgama",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JBangsa_JBangsaId",
                table: "SuPekerja",
                column: "JBangsaId",
                principalTable: "JBangsa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JCaraBayar_JCaraBayarId",
                table: "SuPekerja",
                column: "JCaraBayarId",
                principalTable: "JCaraBayar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SuPekerja_JJawatanPekerja_JJawatanPekerjaId",
                table: "SuPekerja",
                column: "JJawatanPekerjaId",
                principalTable: "JJawatanPekerja",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
