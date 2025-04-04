using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSavvyAsp.net.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeToStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
