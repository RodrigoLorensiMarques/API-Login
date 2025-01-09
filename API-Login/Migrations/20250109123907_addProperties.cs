using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Login.Migrations
{
    /// <inheritdoc />
    public partial class addProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserPassword",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "UserPassword");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "UserName");
        }
    }
}
