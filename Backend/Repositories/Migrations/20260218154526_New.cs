using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Animals");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Animals",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Animals");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Animals",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
