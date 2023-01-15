using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetAPI.Migrations
{
    public partial class AddNewTokenExpireDateColumnToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpireDate",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpireDate",
                table: "Users");
        }
    }
}
