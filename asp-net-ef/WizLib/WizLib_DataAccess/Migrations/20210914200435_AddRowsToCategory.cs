using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddRowsToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO TBL_CATEGORY VALUES('Fiction') ");
            migrationBuilder.Sql("INSERT INTO TBL_CATEGORY VALUES('Education') ");
            migrationBuilder.Sql("INSERT INTO TBL_CATEGORY VALUES('Engineering') ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
