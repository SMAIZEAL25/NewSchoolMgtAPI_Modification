using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace New_School_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class Newupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacultyApproval",
                table: "CourseRegistrations");

            migrationBuilder.AlterColumn<decimal>(
                name: "GPA",
                table: "StudentRecords",
                type: "decimal(4,2)",
                precision: 4,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "CourseRgistration_Id",
                table: "StudentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Local_Goverment_Of_Origin",
                table: "StudentRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State_of_Origin",
                table: "StudentRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Transaction_Id",
                table: "StudentRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseRgistration_Id",
                table: "StudentRecords");

            migrationBuilder.DropColumn(
                name: "Local_Goverment_Of_Origin",
                table: "StudentRecords");

            migrationBuilder.DropColumn(
                name: "State_of_Origin",
                table: "StudentRecords");

            migrationBuilder.DropColumn(
                name: "Transaction_Id",
                table: "StudentRecords");

            migrationBuilder.AlterColumn<double>(
                name: "GPA",
                table: "StudentRecords",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4,
                oldScale: 2);

            migrationBuilder.AddColumn<bool>(
                name: "FacultyApproval",
                table: "CourseRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
