using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class RenateCategoryTableFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "tbl_Category",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Category_Id",
                table: "tbl_Category",
                newName: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tbl_Category",
                newName: "CategoryName");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "tbl_Category",
                newName: "Category_Id");
        }
    }
}
