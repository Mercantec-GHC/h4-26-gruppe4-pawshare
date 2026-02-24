using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class ImageServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Base64Pfp",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Base64Image",
                table: "Animals",
                newName: "AnimalPictureKey");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureKey",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePictureKey",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "AnimalPictureKey",
                table: "Animals",
                newName: "Base64Image");

            migrationBuilder.AddColumn<string>(
                name: "Base64Pfp",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
