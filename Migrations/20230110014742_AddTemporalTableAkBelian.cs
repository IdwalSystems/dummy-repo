using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSNK.Migrations
{
    

    public partial class AddTemporalTableAkBelian : Migration
    {
        public static string GetEnableTemporalTableSql(string tableName) =>
          $@"ALTER TABLE [dbo].[{tableName}]
             ADD PERIOD FOR SYSTEM_TIME (ValidFromUTC, ValidToUTC)
           GO
             ALTER TABLE [dbo].[{tableName}]
             SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE =  [dbo].[{tableName}History], DATA_CONSISTENCY_CHECK = ON))
           GO";

        public static string GetDisableTemporalTableSql(string tableName) =>
          $@"ALTER TABLE [dbo].[{tableName}]
             SET (SYSTEM_VERSIONING = OFF)
            GO
                ALTER TABLE [dbo].[{tableName}]
                DROP PERIOD FOR SYSTEM_TIME
           GO";

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFromUTC",
                table: "AkBelian",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidToUTC",
                table: "AkBelian",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999')");

            migrationBuilder.Sql(GetEnableTemporalTableSql("AkBelian"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(GetDisableTemporalTableSql("AkBelian"));

            migrationBuilder.DropColumn(
                name: "ValidFromUTC",
                table: "AkBelian");

            migrationBuilder.DropColumn(
                name: "ValidToUTC",
                table: "AkBelian");
        }
    }
}
