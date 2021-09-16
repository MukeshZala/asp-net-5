using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib_DataAccess.Migrations
{
    public partial class AddColumnsBirthDateToPublisher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nmae",
                table: "Publishers",
                newName: "Name");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Publishers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Publishers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Publishers",
                newName: "Nmae");
        }
    }
}
