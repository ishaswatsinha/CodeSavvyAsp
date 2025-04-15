using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeSavvyAsp.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorCourseTableFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorEmail",
                table: "InstructorCourses");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "InstructorCourses",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "InstructorId",
                table: "InstructorCourses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InstructorCourses_InstructorId",
                table: "InstructorCourses",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorCourses_Instructors_InstructorId",
                table: "InstructorCourses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorCourses_Instructors_InstructorId",
                table: "InstructorCourses");

            migrationBuilder.DropIndex(
                name: "IX_InstructorCourses_InstructorId",
                table: "InstructorCourses");

            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "InstructorCourses");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "InstructorCourses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "InstructorEmail",
                table: "InstructorCourses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
